using System.Text.RegularExpressions;

namespace Marketplace.Domain;

public partial record ClassifiedAdTitle
{
    private readonly string _value;

    [GeneratedRegex("<.*?>")]
    private static partial Regex UnsupportedHtmlTagsRegex();
    
    public static ClassifiedAdTitle FromString(string title)
    {
        CheckValidity(title);
        return new ClassifiedAdTitle(title);
    }

    public static ClassifiedAdTitle FromHtml(string htmlTitle)
    {
        var supportedTagsReplaced = htmlTitle
            .Replace("<i>", "*")
            .Replace("</i>", "*")
            .Replace("<b>", "**")
            .Replace("</b>", "**");

        var value = UnsupportedHtmlTagsRegex().Replace(supportedTagsReplaced, string.Empty);

        CheckValidity(value);

        return new ClassifiedAdTitle(value);
    }

    internal ClassifiedAdTitle(string value)
    {
        _value = value;
    }

    private static void CheckValidity(string value)
    {
        if (value.Length > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Title cannot be longer than 100 characters");
    }

    public static implicit operator string(ClassifiedAdTitle self) => self._value;
}