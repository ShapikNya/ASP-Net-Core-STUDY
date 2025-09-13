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


app.MapGet("/api/todos", (HttpContext context) =>
{
    if (context.Request.Query.ContainsKey("index"))
    {
        try
        {
            int index = Convert.ToInt32(context.Request.Query["index"]);
            var person = todos.ElementAtOrDefault(index);

            if (person != null)
            {
                return Results.Json(person);
            }
           return Results.BadRequest("Person � ����� �������� �� ����������");
        }
        catch (Exception ex) 
        {
           return Results.BadRequest(ex.Message);
        }
    }

    return Results.Json(todos);
});

app.MapPost("/api/todos", async (HttpContext context) =>
{
    var item = await context.Request.ReadFromJsonAsync<TodoItem>();
    if (item == null) return Results.BadRequest("������ ������");
    todos.Add(item);
    return Results.Ok($"������� {item.Title} ������� ���������!");
});


app.MapPost("/api/upload", async (HttpContext context) =>
{
    try
    {
        IFormFileCollection files = context.Request.Form.Files;

        var uploadPath = $"{Directory.GetCurrentDirectory()}/uploads";

        foreach (var file in files)
        {
            string fullPath = $"{uploadPath}/{file.FileName}";

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }
        return Results.Ok($"������� ��������� {context.Request.Form.Files.Count}!");
    }
    catch (Exception ex)
    {
        return Results.BadRequest($"{ex.Message}");
    }
});

app.Run();

public class TodoItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
}

