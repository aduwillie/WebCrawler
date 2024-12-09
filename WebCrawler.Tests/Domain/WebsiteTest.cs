using WebCrawler.Domain;

namespace WebCrawler.Tests.Domain;

[TestClass]
public class WebsiteTests
{
    [TestMethod]
    public void ShouldCreateWebsite_GivenHttpUri()
    {
        var uri = new Uri("http://www.example.com");
        var website = Website.FromUri(uri);

        Assert.AreEqual(uri, website.Uri);
        Assert.AreEqual(0u, website.Level);
    }

    [TestMethod]
    public void ShouldCreateWebsite_GivenHttpsUri()
    {
        var uri = new Uri("https://www.example.com");
        var website = Website.FromUri(uri);

        Assert.AreEqual(uri, website.Uri);
        Assert.AreEqual(0u, website.Level);
    }

    [TestMethod]
    public void ShouldThrowException_GivenInvalidUri()
    {
        var uri = new Uri("ftp://www.example.com");
        Assert.ThrowsException<ArgumentException>(() => Website.FromUri(uri));
    }

    [TestMethod]
    public void ShouldCreateWebsite_GivenHttpString()
    {
        var uri = "http://aduwillie.com";
        var website = Website.FromString(uri);

        Assert.AreEqual($"{uri}/", website.Uri.ToString());
        Assert.AreEqual(0u, website.Level);
    }

    [TestMethod]
    public void ShouldCreateWebsite_GivenHttpsString()
    {
        var uri = "https://aduwillie.com";
        var website = Website.FromString(uri);

        Assert.AreEqual($"{uri}/", website.Uri.ToString());
        Assert.AreEqual(0u, website.Level);
    }

    [TestMethod]
    public void ShouldCreateWebsiteFromSubpath_GivenValidUri()
    {
        var uri = new Uri("https://aduwillie.com/path1/path2");
        var website = Website.FromUri(uri);

        var subPath = "/path3";
        var newWebsite = website.FromSubPath(subPath);

        Assert.AreEqual(new Uri("https://aduwillie.com/path3"), newWebsite.Uri);
        Assert.AreEqual(1u, newWebsite.Level);
    }
}

