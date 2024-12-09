using System.Net;

namespace WebCrawler.Domain;

internal record HttpError(HttpStatusCode StatusCode, Website Website);
