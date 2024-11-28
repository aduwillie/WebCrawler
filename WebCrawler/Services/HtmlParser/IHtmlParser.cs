namespace WebCrawler.Services.HtmlParser;

internal interface IHtmlParser
{
    List<string> GetLinks(string htmlContent);
}
