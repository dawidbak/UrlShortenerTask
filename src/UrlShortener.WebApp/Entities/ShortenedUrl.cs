namespace UrlShortener.WebApp.Entities;

public class ShortenedUrl
{
    public int Id { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortUrl { get; set; }
}