using WebCrawler.Services.HtmlParser;

namespace WebCrawler.Tests.Services;

[TestClass]
internal class HtmlParserTests
{
    [TestMethod]
    public void ShouldReturnLinks_GivenValidHtml()
    {
        var htmlContent = "<html><body><a href=\"https://example.com\">Link</a></body></html>";
        var parser = new HtmlParser();
        var links = parser.GetLinks(htmlContent);

        Assert.IsTrue(parser is IHtmlParser);
        Assert.AreEqual(1, links.Count);
        Assert.AreEqual("https://example.com", links.First());
    }
}
