namespace Marketplace.Domain.Tests;

public class UserIdTest
{
    [Fact]
    public void Cannot_create_UserId_from_empty_Guid()
    {
        Assert.Throws<ArgumentNullException>(() => new UserId(Guid.Empty));
    }
}