using Marketplace.MinimalApi.Tests.TestContainers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents.Session;

namespace Marketplace.MinimalApi.Tests.Factories;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly IAsyncDocumentSession _documentSession;

    public CustomWebApplicationFactory(IAsyncDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        
        builder.ConfigureServices(services =>
        {
            var storeDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAsyncDocumentSession));
            
            if (storeDescriptor is not null)
                services.Remove(storeDescriptor);

            services.AddSingleton<IAsyncDocumentSession>(_ => _documentSession);
        });
    }
}