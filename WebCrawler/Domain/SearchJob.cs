namespace WebCrawler.Domain;

internal record SearchJob
{
    public Uri Uri { get; private set; } = default!;
    public uint Level { get; private set; } = 0;

    public static SearchJob Create(Website website)
        => new()
        {
            Uri = website.Uri,
            Level = website.Level
        };

    public static SearchJob Create(string url, uint level)
        => Create(Website.FromString(url, level));
}
