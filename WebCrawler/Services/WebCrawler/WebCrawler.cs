using Microsoft.Extensions.Logging;
using WebCrawler.Domain;
using WebCrawler.Services.HtmlParser;

namespace WebCrawler.Services.WebCrawler;

internal class WebCrawler(IHtmlParser htmlParser, HttpClient httpClient, ILogger<WebCrawler> logger)
    : IWebCrawler
{
    private Queue<SearchJob> searchJobQueue = new Queue<SearchJob>();
    private SortedDictionary<string, SearchResult> searchResults = new SortedDictionary<string, SearchResult>();

    public Task<IEnumerable<SearchResult>> Crawl(Website website, int maxPages, CancellationToken cancellationToken)
    {
        logger.LogInformation("Crawling website {Website} up to {MaxPages} pages", website, maxPages);
        searchJobQueue.Enqueue(new SearchJob(website, 0));

        throw new NotImplementedException();
    }

    private async Task<List<string>> DiscoverLinks(Website website)
    {
        var pageContent = await DownloadPage(website);
        logger.LogInformation("Downloaded page {url}, content is {contentLength}", website.ToString(), pageContent?.Length ?? 0);
        if (pageContent is null)
            return new List<string>();

        var links = htmlParser.GetLinks(pageContent)
            .Select(l => l.Trim().Trim('\n'))
            .Select(l => new SearchResult
            {
                ParentPage = website,
                Website = Website.FromString(l),
                OriginalLink = l,
                Level = .Level + 1,
                IsFullyQualified = Uri.IsWellFormedUriString(l, UriKind.Absolute),
            });
        logger.LogInformation("Found {linkCount} links on page {url}", links.Count, job.Website.ToString());
    }

    
    private async Task<string?> DownloadPage(Website website)
    {
        var htmlResponse = await httpClient.GetAsync(website.Uri.AbsoluteUri);
        if (!htmlResponse.IsSuccessStatusCode)
        {
            logger.LogWarning("Failed to download page {Website}. Status code: {StatusCode}", website, htmlResponse.StatusCode);
            return null;
        }
        if (htmlResponse.Content.Headers.ContentType?.MediaType != "text/html")
        {
            logger.LogWarning("Page {Website} is not an HTML page", website);
            return null;
        }

        var content = await htmlResponse.Content.ReadAsStringAsync();
        return content;
    }
}
