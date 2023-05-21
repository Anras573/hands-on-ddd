namespace Marketplace.Domain.Tests;

public class ClassifiedAdIdTest
{
    [Fact]
    public void Cannot_create_ClassifiedAdId_from_empty_Guid()
    {
        Assert.Throws<ArgumentNullException>(() => new ClassifiedAdId(Guid.Empty));
    }
}