using Marketplace.MinimalApi.Application;
using Marketplace.MinimalApi.Endpoints;
using Marketplace.MinimalApi.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplication();
builder.AddInfrastructure();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ClassifiedAds",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds v1");
    });
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!")
    .WithName("Hello World!")
    .WithOpenApi();

ClassifiedAdsCommandsEndpoints.Map(app);

app.Run();

public partial class Program { } // So I can reference it from tests.