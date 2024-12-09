using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using WebCrawler.Domain;
using WebCrawler.Services.HtmlParser;

namespace WebCrawler.Services.WebCrawler;

internal class WebCrawler(IHtmlParser htmlParser, HttpClient httpClient, ILogger<WebCrawler> logger)
    : IWebCrawler
{
    private const int RetryAttempts = 3;
    private Queue<SearchJob> searchJobQueue = new Queue<SearchJob>();
    private SortedDictionary<string, SearchResult> searchResults = new SortedDictionary<string, SearchResult>();

    public async Task<IEnumerable<SearchResult>> Crawl(Website website, int maxPages, CancellationToken cancellationToken)
    {
        logger.LogInformation("Crawling website {Website} up to {MaxPages} pages", website, maxPages);
        searchJobQueue.Enqueue(SearchJob.Create(website));

        var processedPages = 0;
        while (processedPages < maxPages)
        {
            if (searchJobQueue.Count == 0)
            {
                logger.LogInformation("No more pages to crawl.");
                break;
            }

            await DiscoverLinks(searchJobQueue.Dequeue());

            processedPages++;
        }

        return searchResults.Values;
    }

    private async Task DiscoverLinks(SearchJob job)
    {
        var website = Website.FromUri(job.Uri, job.Level);
        var pageContent = await DownloadPage(website);
        logger.LogInformation("Downloaded page {url}, content is {contentLength}", website.ToString(), pageContent?.Length ?? 0);

        if (pageContent is null) return;

        var links = htmlParser.GetLinks(pageContent)
            .Select(l => l.Trim().Trim('\n'))
            .Select(l => new SearchResultBuilder()
                .AddOriginalLink(l)
                .AddParentPage(website)
                .Build())
            .ToList();
        logger.LogInformation($"Found {links.Count} links on page {website.ToString()}");

        // Add the link to the relevant dictionaries
        foreach (var link in links)
        {
            if (searchResults.ContainsKey(link.OriginalLink))
            {
                logger.LogInformation("Link {link} already discovered", link.OriginalLink);
                continue;
            }
            searchResults.Add(link.Website.ToString(), link);
            searchJobQueue.Enqueue(SearchJob.Create(link.Website));
        }
    }


    private async Task<string?> DownloadPage(Website website)
    {
        // Leverage Polly to handle transient errors
        var retryPolicy = CreateExponentialBackoffPolicy();
        var htmlResponse = await retryPolicy
            .ExecuteAsync(() => httpClient.GetAsync(website.Uri.AbsoluteUri));
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

    private static AsyncRetryPolicy<HttpResponseMessage> CreateExponentialBackoffPolicy()
    {
        var unAcceptableResponses = new HttpStatusCode[] { HttpStatusCode.GatewayTimeout, HttpStatusCode.GatewayTimeout };
        return Policy
            .HandleResult<HttpResponseMessage>(resp => unAcceptableResponses.Contains(resp.StatusCode))
            .WaitAndRetryAsync(
            RetryAttempts,
            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }
}
