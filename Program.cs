using ASP_study.Middleware;
using Microsoft.Extensions.FileProviders;
using System;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.UseLogging();
app.UseExceptionMiddleware();
app.MapControllers();

app.Run();

