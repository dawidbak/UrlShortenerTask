using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.WebApp.Exceptions;
using UrlShortener.WebApp.Infrastructure;

namespace UrlShortener.WebApp.Features.ShortenedUrl;

public record GetOriginalUrl(string Url) : IRequest<string>;

internal class GetOriginalUrlValidator : AbstractValidator<GetOriginalUrl>
{
    public GetOriginalUrlValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage("Url is required");
    }
}

public class GetOriginalUrlHandler : IRequestHandler<GetOriginalUrl, string>
{
    private readonly UrlShortenerDbContext _dbContext;

    public GetOriginalUrlHandler(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> Handle(GetOriginalUrl request, CancellationToken cancellationToken)
    {
        var originalUrl = await _dbContext.ShortenedUrls
            .Where(x => x.ShortUrl == request.Url)
            .Select(x => x.OriginalUrl)
            .FirstOrDefaultAsync(cancellationToken);

        if (originalUrl is null)
            throw new ShortenedUrlNotFoundException();

        return originalUrl;
    }
};