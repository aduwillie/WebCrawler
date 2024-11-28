using WebCrawler.Domain;

namespace WebCrawler.Services.WebCrawler;

internal interface IWebCrawler
{
    Task<IEnumerable<SearchResult>> Crawl(Website website, int maxPages, CancellationToken cancellationToken);
}
