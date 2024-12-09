namespace WebCrawler.Domain;

internal record SearchResult
{
    public Website Website { get; init; } = default!;
    public string OriginalLink { get; init; } = string.Empty;
    public bool IsExternalDomain { get; init; }
    public bool IsFullyQualified { get; init; }
    public Website? ParentPage { get; init; }

    public override string ToString() => 
        $"Parent={ParentPage?.ToString()} Child={Website.ToString()} Level={Website.Level} IsFullyQualified={IsFullyQualified}";
}

internal class SearchResultBuilder
{
    private string _originalLink = string.Empty;
    private bool _isExternalWebsite = false;
    private bool _isFullyQualified;
    private Website? _parentPage;

    public SearchResultBuilder AddOriginalLink(string originalLink)
    {
        ArgumentException.ThrowIfNullOrEmpty(originalLink);
        _originalLink = originalLink;

        // Set IsFullyQualified based on the original link
        if (originalLink.StartsWith("/"))
            _isFullyQualified = false;
        else
            _isFullyQualified = true;

        return this;
    }

    public SearchResultBuilder AddParentPage(Website parentPage)
    {
        _parentPage = parentPage;
        return this;
    }
    public SearchResult Build()
    {
        var level = _parentPage is null ? 0 : _parentPage.Level + 1;
        var website = _originalLink switch
        {
            var o when o.StartsWith("/") && _parentPage is null => throw new Exception("Parent page cannot be null for relative links"),
            var o when o.StartsWith("/") && _parentPage is not null => _parentPage.FromSubPath(_originalLink),
            _ => Website.FromString(_originalLink, level)
        };

        var isExternalWebsite = _parentPage is not null && _parentPage.Uri.Host != website.Uri.Host;

        return new SearchResult
        {
            Website = website,
            OriginalLink = _originalLink,
            IsExternalDomain = _isExternalWebsite,
            IsFullyQualified = _isFullyQualified,
            ParentPage = _parentPage
        };
    }
}