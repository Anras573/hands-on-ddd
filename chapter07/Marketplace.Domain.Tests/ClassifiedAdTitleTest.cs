namespace Marketplace.Domain.Tests;

public class ClassifiedAdTitleTest
{
    [Fact]
    public void Reject_titles_longer_than_100_characters_in_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new ClassifiedAdTitle(new string('a', 101)));
    }
    
    [Fact]
    public void Allow_titles_100_characters_or_less_in_length()
    {
        var title = new ClassifiedAdTitle(new string('a', 100));
        var expected = new string('a', 100);
        Assert.Equal(expected, title);
    }
    
    [Fact]
    public void Allow_html_titles()
    {
        var title = ClassifiedAdTitle.FromHtml("<b>test</b> and <i>another</i> test");
        Assert.Equal("**test** and *another* test", title);
    }
    
    [Fact]
    public void Reject_html_titles_longer_than_100_characters_in_length()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ClassifiedAdTitle.FromHtml(new string('a', 101)));
    }
    
    [Fact]
    public void Discard_unsupported_html_tags()
    {
        var title = ClassifiedAdTitle.FromHtml("<script>alert('test');</script><b>test</b>");
        Assert.Equal("alert('test');**test**", title);
    }
}