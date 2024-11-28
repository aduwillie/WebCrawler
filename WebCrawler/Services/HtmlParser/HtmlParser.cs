using HtmlAgilityPack;

namespace WebCrawler.Services.HtmlParser;

internal class HtmlParser : IHtmlParser
{
    public List<string> GetLinks(string htmlContent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(htmlContent, nameof(htmlContent));

        // Load the HTML content into an HtmlDocument
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        // Select all anchor tags
        return htmlDocument.DocumentNode.SelectNodes("//a[@href]")
            .Where(n => n.Attributes.Contains("href"))
            .Select(n => n.Attributes["href"])
            .Select(a => a.Value)
            .ToList();
    }
}
