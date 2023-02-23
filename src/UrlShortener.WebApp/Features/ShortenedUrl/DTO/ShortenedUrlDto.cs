namespace UrlShortener.WebApp.Features.ShortenedUrl;

public class ShortenedUrlDto
{
    public string ShortUrl { get; init; } = default!;
    public string OriginalUrl { get; init; } = default!;
}