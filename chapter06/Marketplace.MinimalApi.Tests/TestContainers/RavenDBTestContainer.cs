using DotNet.Testcontainers.Builders;
using Marketplace.Domain;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Marketplace.MinimalApi.Tests.TestContainers;

public class RavenDBTestContainer
{
    public static async Task<IAsyncDocumentSession> CreateDocumentSessionAsync()
    {
        var container = new ContainerBuilder()
            .WithImage("ravendb/ravendb:5.4-ubuntu-latest")
            .WithPortBinding(8080, true)
            .Build();

        await container.StartAsync();

        var url = $"http://{container.Hostname}:{container.GetMappedPublicPort(8080)}";
        var store = SetupDocumentStore(url);
        
        return store.OpenAsyncSession();
    }
    
    private static DocumentStore SetupDocumentStore(string url)
    {
        const string databaseName = "Marketplace_Chapter6";

        var store = new DocumentStore
        {
            Urls = new[] { url },
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