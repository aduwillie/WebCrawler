using WebCrawler.Domain;

namespace WebCrawler.Tests.Domain;

[TestClass]
internal class SearchJobTests
{
    [TestMethod]
    public void ShouldCreateSearchJob_GivenWebsite()
    {
        var website = Website.FromString("https://aduwillie.com");
        var searchJob = SearchJob.Create(website);

        Assert.AreEqual(website.Uri, searchJob.Uri);
        Assert.AreEqual(website.Level, searchJob.Level);
    }

    [TestMethod]
    public void ShouldCreateSearchJob_GivenUrlAndLevel()
    {
        var url = "https://aduwillie.com";
        var level = 1u;
        var searchJob = SearchJob.Create(url, level);

        Assert.AreEqual(new Uri(url), searchJob.Uri);
        Assert.AreEqual(level, searchJob.Level);
    }
}
