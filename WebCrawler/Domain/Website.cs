namespace WebCrawler.Domain;

internal record Website
{
    public Uri Uri { get; private set; } = default!;
    public uint Level { get; private set; } = 0;

    public static Website FromUri(Uri uri, uint level = 0)
    {
        var isValidUri = uri.Scheme switch
        {
            var scheme when scheme == Uri.UriSchemeHttp => true,
            var scheme when scheme == Uri.UriSchemeHttps => true,
            _ => false
        };
        if (!isValidUri)
            throw new ArgumentException("Invalid URI scheme. Only HTTP and HTTPS are supported.", nameof(uri));
        
        return new Website
        {
            Uri = uri,
            Level = level
        };
    }

    public static Website FromString(string absoluteUrl, uint level = 0)
    {
        ArgumentException.ThrowIfNullOrEmpty(absoluteUrl, nameof(absoluteUrl));

        if (Uri.TryCreate(absoluteUrl, UriKind.Absolute, out var uri) && uri is not null)
            return new Website
            {
                Uri = uri,
                Level = level
            };

        throw new ArgumentException($"Invalid URL: {absoluteUrl}");
    }

    public Website FromSubPath(string websiteSubPath)
    {
        // Check if input is null or empty
        ArgumentException.ThrowIfNullOrEmpty(websiteSubPath, nameof(websiteSubPath));

        // Ensure the subpath starts with a "/"
        if (!websiteSubPath.StartsWith("/"))
            throw new ArgumentException("Subpath must start with a '/'", nameof(websiteSubPath));

        // Create a new URI by combining the base URI with the subpath
        var reformedUri = new Uri($"{Uri.Scheme}://{Uri.Host}{websiteSubPath}");
        var newLevel = Level + 1;

        return new Website
        {
            Uri = reformedUri,
            Level = newLevel,
        };
    }

    public override string ToString() => Uri switch
    {
        not null => Uri.AbsoluteUri,
        null => throw new Exception("Website URI is not set."),
    };
}
