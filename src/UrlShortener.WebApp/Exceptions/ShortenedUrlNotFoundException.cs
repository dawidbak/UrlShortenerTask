namespace UrlShortener.WebApp.Exceptions;

public class ShortenedUrlNotFoundException : Exception
{
    public ShortenedUrlNotFoundException() : base("Shortened URL not found")
    {
    }
}