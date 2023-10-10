using Marketplace.Domain;
using Marketplace.MinimalApi.Application;
using Marketplace.MinimalApi.Infrastructure;
using Marketplace.MinimalApi.Models;
using Marketplace.MinimalApi.Tests.Fakes;
using Marketplace.MinimalApi.Tests.TestContainers;

namespace Marketplace.MinimalApi.Tests.Application;

[Trait("Type", "Integration")]
public class ClassifiedAdsApplicationServiceTests
{
    [Fact]
    public async Task GivenCreateCommand_WhenHandle_ThenClassifiedAdIsCreated()
    {
        // Arrange
        var session = await RavenDBTestContainer.CreateDocumentSessionAsync();
        var repository = new ClassifiedAdRepository(session);
        var sut = new ClassifiedAdsApplicationService(repository, new FakeCurrencyLookup());
        var command = new ClassifiedAds.V1.Create
        {
            Id = new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f"),
            OwnerId = new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a")
        };

        // Act
        await sut.Handle(command);

        // Assert
        Assert.True(repository.Exists(new ClassifiedAdId(command.Id)).Result);
        var classifiedAd = await repository.Load(new ClassifiedAdId(command.Id));
        Assert.Equal(command.Id, classifiedAd.Id);
        Assert.Equal(command.OwnerId, classifiedAd.OwnerId);
        Assert.Equal(command.OwnerId, classifiedAd.OwnerId);
    }

    [Fact]
    public async Task WhenClassifiedAdExists_GivenCreateCommand_WhenHandle_ThenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var session = await RavenDBTestContainer.CreateDocumentSessionAsync();
        var repository = new ClassifiedAdRepository(session);

        await repository.Save(new ClassifiedAd(new ClassifiedAdId(new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f")), new UserId(new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a"))));
        
        var sut = new ClassifiedAdsApplicationService(repository, new FakeCurrencyLookup());
        var command = new ClassifiedAds.V1.Create
        {
            Id = new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f"),
            OwnerId = new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a")
        };

        // Act
        var action = async () => await sut.Handle(command);

        // Assert
        await Assert.ThrowsAsync<InvalidOperationException>(action);
    }
}