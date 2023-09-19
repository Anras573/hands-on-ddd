using System.Net;
using System.Net.Http.Json;
using Marketplace.MinimalApi.Models;
using Marketplace.MinimalApi.Tests.Factories;


namespace Marketplace.MinimalApi.Tests.Endpoints;

public class ClassifiedAdsCommandsEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    
    public ClassifiedAdsCommandsEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact]
    [Trait("Type", "Integration")]
    public async Task GivenCreateClassifiedAd_WhenPost_Then200OK()
    {
        var command = new ClassifiedAds.V1.Create
        {
            Id = new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f"),
            OwnerId = new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a")
        };
        
        var response = await _client.PostAsJsonAsync("/ad", command);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}