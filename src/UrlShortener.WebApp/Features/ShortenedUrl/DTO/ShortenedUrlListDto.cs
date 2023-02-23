namespace UrlShortener.WebApp.Features.ShortenedUrl.DTO;

public class ShortenedUrlListDto
{
    public IEnumerable<ShortenedUrlDto> ShortenedUrls { get; init; } = default!;
}