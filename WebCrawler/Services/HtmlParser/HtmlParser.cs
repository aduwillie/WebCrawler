using WebCrawler.Extensions;

namespace WebCrawler.Services.HtmlParser;

internal class HtmlParser : IHtmlParser
{
    public List<string> GetLinks(string htmlContent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(htmlContent, nameof(htmlContent));

        var isValid = htmlContent.IsValidHtml(out var htmlDocument);
        if (!isValid)
        {
            throw new ArgumentException("Invalid HTML content", nameof(htmlContent));
        }

        // Select all anchor tags
        return htmlDocument.DocumentNode.SelectNodes("//a[@href]")
            .Where(n => n.Attributes.Contains("href"))
            .Select(n => n.Attributes["href"])
            .Select(a => a.Value)
            .ToList();
    }
}
