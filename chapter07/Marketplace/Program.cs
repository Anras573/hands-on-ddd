using Marketplace;
using Marketplace.Api;
using Marketplace.Domain;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

const string databaseName = "Marketplace_Chapter7";

var store = new DocumentStore
{
    Urls = new[] { "http://localhost:8080" },
    Database = databaseName,
    Conventions =
    {
        FindIdentityProperty = m => m.Name == "_databaseId"
    }
};

store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>((dbName, entity) => Task.FromResult($"ClassifiedAd/{entity.Id}"));
store.Initialize();

if (!store.Maintenance.Server.Send(new GetDatabaseNamesOperation(0, 10)).Contains(databaseName))
{
    store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(databaseName)));
}

builder.Services.AddTransient(_ => store.OpenAsyncSession());
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddScoped<ClassifiedAdsApplicationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
