using Microsoft.Extensions.FileProviders;
using System;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWelcomePage();

app.Run();

