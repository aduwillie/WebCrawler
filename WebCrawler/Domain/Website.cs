namespace WebCrawler.Domain;

internal record Website(Uri Uri, int Level)
{
    public Uri Uri { get; set; } =
        Uri.Scheme != Uri.UriSchemeHttp || Uri.Scheme != Uri.UriSchemeHttps
        ? throw new ArgumentException("Invalid URI")
        : Uri; 
    
    public Website FromSubPath(string subPath)
    {
        if (subPath.StartsWith("/"))
        {
            var protocol = Uri.Scheme;
            var host = Uri.Host;

            return new Website(new Uri($"{protocol}://{host}{subPath}"), Level + 1);
        }
        else if (Uri.TryCreate(subPath, UriKind.Absolute, out var subPathUri))
        {
            // Ignore if host is different from parent
            if (subPathUri?.Host != Uri.Host)
                throw new Exception("Going to a different domain");

            return new Website(subPathUri, Level + 1);
        }

        throw new Exception($"Invalid sub path: {subPath}");
    }

    public override string ToString() => Uri switch
    {
        not null => Uri.AbsoluteUri,
        null => throw new Exception("Website URI is not set."),
    };

    public static Website FromString(string absoluteUrl, int level)
    {
        ArgumentException.ThrowIfNullOrEmpty(absoluteUrl, nameof(absoluteUrl));

        if (Uri.TryCreate(absoluteUrl, UriKind.Absolute, out var uri) && uri is not null)
            return new Website(Uri: uri, Level: level);

        throw new ArgumentException($"Invalid URL: {absoluteUrl}");
    }
}
