using Marketplace.Domain;
using Marketplace.MinimalApi.Tests.Fakes;
using Raven.Client.Documents.Session;

namespace Marketplace.MinimalApi.Tests.Factories.Seeds;

public static class ClassifiedAdSeeder
{
    public static async Task SeedAsync(IAsyncDocumentSession session)
    {
        foreach (var ad in Ads())
        {
            await session.StoreAsync(ad);
        }
        
        await session.SaveChangesAsync();
    }

    private static IEnumerable<ClassifiedAd> Ads()
    {
        var defaultUserId = new Guid("ae639869-b4d4-4c5c-8441-fb6228dd8a4a");
        
        yield return CreateClassifiedAd(new Guid("34b7e345-b244-4c7e-9b17-13327f0ba272"), defaultUserId, "Title", "Description", 100);
        yield return CreateClassifiedAd(new Guid("4ad64235-5704-415d-b1be-4c4cdba7e9be"), defaultUserId, "Test Title", "Test Description", 120);
        yield return CreateClassifiedAd(new Guid("c5de0e6d-f516-453f-a424-b44783eabf73"), defaultUserId, "Title of Classified Ad", "Description of Classified Ad", 250);
    }

    private static ClassifiedAd CreateClassifiedAd(Guid id, Guid userId, string title, string text, decimal price)
    {
        var ad = new ClassifiedAd(new ClassifiedAdId(id), new UserId(userId));
        ad.SetTitle(new ClassifiedAdTitle(title));
        ad.UpdateText(ClassifiedAdText.FromString(text));
        ad.UpdatePrice(Price.FromDecimal(price, "EUR", new FakeCurrencyLookup()));

        return ad;
    }
}