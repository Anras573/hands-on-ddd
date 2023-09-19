using Marketplace.Domain;
using Marketplace.MinimalApi.Infrastructure;
using Marketplace.MinimalApi.Tests.TestContainers;

namespace Marketplace.MinimalApi.Tests.Infrastructure;

public class ClassifiedAdRepositoryTests
{
    [Fact]
    [Trait("Type", "Integration")]
    public async Task GivenClassifiedAdDoesNotExist_WhenExists_ThenReturnsFalse()
    {
        // Arrange
        var session = await RavenDBTestContainer.CreateDocumentSessionAsync();
        var repository = new ClassifiedAdRepository(session);

        // Act
        var result = await repository.Exists(new ClassifiedAdId(new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f")));

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    [Trait("Type", "Integration")]
    public async Task GivenClassifiedAdExists_WhenExists_ThenReturnsTrue()
    {
        // Arrange
        var session = await RavenDBTestContainer.CreateDocumentSessionAsync();
        var repository = new ClassifiedAdRepository(session);
        var classifiedAd = new ClassifiedAd(new ClassifiedAdId(new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f")), new UserId(new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a")));

        // Act
        await repository.Save(classifiedAd);
        var result = await repository.Exists(classifiedAd.Id);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    [Trait("Type", "Integration")]
    public async Task GivenClassifiedAd_WhenLoadingClassifiedAd_ThenClassifiedAdIsLoaded()
    {
        // Arrange
        var session = await RavenDBTestContainer.CreateDocumentSessionAsync();
        var repository = new ClassifiedAdRepository(session);
        var classifiedAd = new ClassifiedAd(new ClassifiedAdId(new Guid("d8c0d4e8-7d1f-4e00-8b5a-0e2e9e9e1f1f")), new UserId(new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a")));

        // Act
        await repository.Save(classifiedAd);
        var loadedAd = await repository.Load(classifiedAd.Id);

        // Assert
        Assert.Equal(classifiedAd, loadedAd);
    }
}