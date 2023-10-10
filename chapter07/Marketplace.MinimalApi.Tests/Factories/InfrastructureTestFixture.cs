using Marketplace.MinimalApi.Tests.Factories.Seeds;
using Marketplace.MinimalApi.Tests.TestContainers;
using Raven.Client.Documents.Session;

namespace Marketplace.MinimalApi.Tests.Factories;

public class InfrastructureTestFixture : IAsyncLifetime
{
    public IAsyncDocumentSession DocumentSession { get; private set; }
    
    public async Task InitializeAsync()
    {
        var session = await RavenDBTestContainer.CreateDocumentSessionAsync();

        await ClassifiedAdSeeder.SeedAsync(session);
        
        DocumentSession = session;
    }

    public Task DisposeAsync()
    {
        DocumentSession?.Dispose();
        
        return Task.CompletedTask;
    }
}