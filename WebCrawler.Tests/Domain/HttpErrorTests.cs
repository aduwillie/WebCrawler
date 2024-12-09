using System.Net;
using WebCrawler.Domain;

namespace WebCrawler.Tests.Domain;

[TestClass]
public class HttpErrorTests
{
    [TestMethod]
    public void ShouldCreateHttpError_GivenStatusCodeAndWebsite()
    {
        var statusCode = HttpStatusCode.NotFound;
        var website = Website.FromString("https://example.com");
        var httpError = new HttpError(statusCode, website);

        Assert.AreEqual(statusCode, httpError.StatusCode);
        Assert.AreEqual(website, httpError.Website);
    }
}
