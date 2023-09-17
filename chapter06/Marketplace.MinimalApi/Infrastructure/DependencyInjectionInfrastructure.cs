using Marketplace.Domain;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Marketplace.MinimalApi.Infrastructure;

public static class DependencyInjectionInfrastructure
{
    public static void AddInfrastructure(this WebApplicationBuilder builder)
    {
        var store = SetupDocumentStore();

        builder.Services.AddTransient(_ => store.OpenAsyncSession());
        builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
    }

    private static DocumentStore SetupDocumentStore()
    {
        const string databaseName = "Marketplace_Chapter6";

        var store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = databaseName,
            Conventions =
            {
                FindIdentityProperty = m => m.Name == "_databaseId"
            }
        };

        store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>((dbName, entity) =>
            Task.FromResult($"ClassifiedAd/{entity.Id}"));
        store.Initialize();

        if (!store.Maintenance.Server.Send(new GetDatabaseNamesOperation(0, 10)).Contains(databaseName))
        {
            store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(databaseName)));
        }

        return store;
    }
}