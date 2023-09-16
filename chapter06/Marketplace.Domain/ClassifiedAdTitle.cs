using System.Text.RegularExpressions;

namespace Marketplace.Domain;

public partial record ClassifiedAdTitle
{
    public string Title { get; }

    [GeneratedRegex("<.*?>")]
    private static partial Regex UnsupportedHtmlTagsRegex();

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace("<i>", "*")
            .Replace("</i>", "*")
            .Replace("<b>", "**")
            .Replace("</b>", "**");

        var value = UnsupportedHtmlTagsRegex().Replace(supportedTagsReplaced, string.Empty);
        
        return new ClassifiedAdTitle(value);
    }
    
    public ClassifiedAdTitle(string title)
    {
        if (title.Length > 100)
                 throw new ArgumentOutOfRangeException(nameof(title), "Title cannot be longer than 100 characters");
        
        Title = title;
    }

    public static implicit operator string(ClassifiedAdTitle self) => self.Title;
}