using HtmlAgilityPack;

namespace WebCrawler.Extensions;

internal static class HtmlDocumentExtensions
{
    public static bool IsValidHtml(this string html, out HtmlDocument htmlDocument)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(html, nameof(html));
        htmlDocument = new HtmlDocument();
        try
        {
            htmlDocument.LoadHtml(html);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
