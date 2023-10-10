using System.Net;
using System.Net.Http.Json;
using Marketplace.MinimalApi.Models;
using Marketplace.MinimalApi.Tests.Factories;

namespace Marketplace.MinimalApi.Tests.Endpoints;

[Trait("Type", "Integration")]
public class ClassifiedAdsCommandsEndpointsTests : IClassFixture<InfrastructureTestFixture>
{
    private readonly HttpClient _client;
    
    public ClassifiedAdsCommandsEndpointsTests(InfrastructureTestFixture fixture)
    {
        var factory = new CustomWebApplicationFactory(fixture.DocumentSession);
        _client = factory.CreateClient();
    }
    
    [Fact]
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
    
    [Fact]
    public async Task GivenSetTitle_WhenPut_Then200OK()
    {
        var command = new ClassifiedAds.V1.SetTitle
        {
            Id = new Guid("34b7e345-b244-4c7e-9b17-13327f0ba272"),
            Title = "Test"
        };
        
        var response = await _client.PutAsJsonAsync("/ad/name", command);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GivenSetText_WhenPut_Then200OK()
    {
        var command = new ClassifiedAds.V1.UpdateText
        {
            Id = new Guid("4ad64235-5704-415d-b1be-4c4cdba7e9be"),
            Text = "Test"
        };
        
        var response = await _client.PutAsJsonAsync("/ad/text", command);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GivenUpdatePrice_WhenPut_Then200OK()
    {
        var command = new ClassifiedAds.V1.UpdatePrice
        {
            Id = new Guid("c5de0e6d-f516-453f-a424-b44783eabf73"),
            Price = 1234,
            CurrencyCode = "USD"
        };
        
        var response = await _client.PutAsJsonAsync("/ad/price", command);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GivenRequestToPublish_WhenPut_Then200OK()
    {
        var command = new ClassifiedAds.V1.RequestToPublish
        {
            Id = new Guid("c5de0e6d-f516-453f-a424-b44783eabf73")
        };
        
        var response = await _client.PutAsJsonAsync("/ad/publish", command);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}