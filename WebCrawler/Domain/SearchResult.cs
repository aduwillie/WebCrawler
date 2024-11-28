namespace WebCrawler.Domain;

internal record SearchResult
{
    public Website Website { get; set; } = default!;
    public string OriginalLink { get; set; } = string.Empty;
    public bool IsExternalDomain { get; set; }
    public bool IsFullyQualified { get; set; }
    public int Level { get; set; } = 0;
    public Website? ParentPage { get; set; }

    public override string ToString() => 
        $"Parent={ParentPage?.ToString()} Child={Website.ToString()} Level={Level} IsFullyQualified={IsFullyQualified}";
}
