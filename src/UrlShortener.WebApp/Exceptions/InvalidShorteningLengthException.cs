namespace UrlShortener.WebApp.Exceptions;

public class InvalidShorteningLengthException : Exception
{
    public InvalidShorteningLengthException(int length) : base($"Shortening length must be equal or above 1, but was {length}.")
    {
    }
}