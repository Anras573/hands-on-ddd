using Marketplace.Domain;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Marketplace.MinimalApi.Infrastructure;

public sealed class InfrastructureOptions
{
    public bool CreateDatabase { get; set; }
    public string DatabaseName { get; set; }
}

public static class DependencyInjectionInfrastructure
{
    public static void AddInfrastructure(this WebApplicationBuilder builder, Func<InfrastructureOptions> optionsFactory)
    {
        var options = optionsFactory();
        var store = SetupDocumentStore(options);

        builder.Services.AddSingleton(_ => store.OpenAsyncSession());
        builder.Services.AddSingleton<IClassifiedAdRepository, ClassifiedAdRepository>();
    }

    private static DocumentStore SetupDocumentStore(InfrastructureOptions options)
    {
        var store = new DocumentStore
        {
            Urls = new[] { "http://localhost:8080" },
            Database = options.DatabaseName,
            Conventions =
            {
                FindIdentityProperty = m => m.Name == "_databaseId"
            }
        };

        store.Conventions.RegisterAsyncIdConvention<ClassifiedAd>((dbName, entity) =>
            Task.FromResult($"ClassifiedAd/{entity.Id}"));
        store.Initialize();

        if (!options.CreateDatabase) return store;
        
        if (!store.Maintenance.Server.Send(new GetDatabaseNamesOperation(0, 10)).Contains(options.DatabaseName))
        {
            store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(options.DatabaseName)));
        }
        
        return store;
    }
}