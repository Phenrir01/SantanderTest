using FluentValidation;
using HackerNewsGetterAPI.Exceptions;
using HackerNewsGetterAPI.Extensions;
using HackerNewsGetterAPI.Features.BestStories.GetAllBestStories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClientConfiguration(builder.Configuration);
builder.Services.AddHandlersFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddValidatorsFromAssembly(typeof(GetAllBestStoriesValidator).Assembly);
builder.Services.AddMemoryCache();
builder.Services.AddExceptionHandler<CustomExceptionHandler>()
            .AddProblemDetails();


var app = builder.Build();

app.MapEndpoints();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapGet("/", () => Results.Redirect("/swagger"));
app.Run();