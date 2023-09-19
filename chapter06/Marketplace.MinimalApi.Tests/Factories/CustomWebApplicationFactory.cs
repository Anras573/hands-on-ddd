using Marketplace.MinimalApi.Tests.TestContainers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Raven.Client.Documents.Session;

namespace Marketplace.MinimalApi.Tests.Factories;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var storeDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAsyncDocumentSession));
            
            if (storeDescriptor is not null)
                services.Remove(storeDescriptor);
            
            services.AddTransient(_ => RavenDBTestContainer.CreateDocumentSessionAsync().GetAwaiter().GetResult());
        });
    }
}