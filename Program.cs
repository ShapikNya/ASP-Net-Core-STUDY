using Microsoft.Extensions.FileProviders;
using System;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var todos = new List<TodoItem>() {
    new TodoItem { Id = Guid.NewGuid(), Title ="homeWork", IsCompleted = false },
    new TodoItem{Id = Guid.NewGuid(), Title = "Gym", IsCompleted = true},
    new TodoItem{Id = Guid.NewGuid(), Title = "Clean room", IsCompleted = false}
};


app.Run(async (context) =>
{
    var request = context.Request;
    var response = context.Response; response.ContentType = "text/html; charset=utf-8";

    switch (request.Method)
    {
        case "GET":
            {
                if (request.Path != "/api/todos") break;

                //Поиск по индексу
                if (request.Query.ContainsKey("index"))
                {
                    try
                    {
                        int index = Convert.ToInt32(request.Query["index"]);
                        var person = todos.ElementAtOrDefault(index);
                        if (person != null)
                        {
                            string personJson = JsonSerializer.Serialize(person, new JsonSerializerOptions
                            {
                                WriteIndented = true
                            });
                            response.StatusCode = 201;
                            await response.WriteAsync(personJson);
                        }
                        response.StatusCode = 400;
                        await response.WriteAsync("Person с таким индексом не существует");
                        break;
                    }
                    catch
                    {
                        response.StatusCode = 400;
                        await response.WriteAsync("Неверная запись значения ключа");
                        break;
                    }
                }
                //Вывод всех заданий
                string Json = JsonSerializer.Serialize(todos, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await response.WriteAsync(Json);
                break;
            }

        case "POST":
            {
                if (request.Path == "/api/todos")
                {
                    try
                    {
                        var item = await request.ReadFromJsonAsync<TodoItem>();
                        if (item == null) return;
                        todos.Add(item);
                        response.StatusCode = 201;
                        await response.WriteAsync($"Задание {item.Title} успешно добавлено!");
                    }
                    catch
                    {
                        response.StatusCode = 400;
                        await response.WriteAsync("Неверная запись Json");
                    }
                }

                if (request.Path == "/api/upload")
                {
                    IFormFileCollection files = request.Form.Files;

                    var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";

                    foreach (var file in files)
                    {
                        string fullPath = $"{uploadPath}/{file.FileName}";

                        using (var fileStream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    await response.WriteAsync("Файлы успешно загружены");

                }


                break;
            }
        default:
            {
                response.StatusCode = 500;
                response.WriteAsync("Тип данного запроса не поддерживается"); break;
            }
    }


});

app.Run();

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

