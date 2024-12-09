using WebCrawler.Domain;

namespace WebCrawler.Tests.Domain;

[TestClass]
internal class SearchResultTests
{
    [TestMethod]
    public void ShouldCreateSearchResult_GivenWebsiteAndLinks()
    {
        var originalLink = "https://aduwillie.com/path1";
        var parentLink = "https://aduwillie.com";

        var searchResult = new SearchResultBuilder()
            .AddOriginalLink(originalLink)
            .AddParentPage(Website.FromString(parentLink, 2))
            .Build();

        Assert.AreEqual(originalLink, searchResult.OriginalLink);
        Assert.AreEqual(parentLink, searchResult.ParentPage?.ToString());
        Assert.IsFalse(searchResult.IsExternalDomain);
        Assert.IsTrue(searchResult.IsFullyQualified);
    }
}
