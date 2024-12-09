using WebCrawler.Extensions;

namespace WebCrawler.Tests.Extensions;

[TestClass]
internal class HtmlDocumentExtensionsTest
{
    [TestMethod]
    public void ShouldReturnTrue_GivenValidHtml()
    {
        var html = "<html><body><h1>Test</h1></body></html>";
        var result = html.IsValidHtml(out var htmlDocument);
        Assert.IsTrue(result);
        Assert.IsNotNull(htmlDocument);
    }

    [TestMethod]
    public void ShouldReturnFalse_GivenInvalidHtml()
    {
        var html = "<html><body><h1>Test</body></html>";
        var result = html.IsValidHtml(out var htmlDocument);
        Assert.IsFalse(result);
        Assert.IsNull(htmlDocument);
    }
}
