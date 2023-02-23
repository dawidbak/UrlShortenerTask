using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.WebApp.Common;
using UrlShortener.WebApp.Exceptions;
using UrlShortener.WebApp.Infrastructure;
using UrlShortener.WebApp.Settings;

namespace UrlShortener.WebApp.Features.ShortenedUrl;

public record AddShortenedUrl(string Url) : IRequest<ShortenedUrlDto>;

public class AddShortenedUrlValidator : AbstractValidator<AddShortenedUrl>
{
    public AddShortenedUrlValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out var result) &&
                result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps)
            .WithMessage("Url is not valid");
    }
}

public class AddShortenedUrlHandler : IRequestHandler<AddShortenedUrl, ShortenedUrlDto>
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly ShorteningSettings _shorteningSettings;

    public AddShortenedUrlHandler(UrlShortenerDbContext dbContext, IOptions<ShorteningSettings> shorteningSettings)
    {
        _dbContext = dbContext;
        _shorteningSettings = shorteningSettings.Value;
    }

    public async Task<ShortenedUrlDto> Handle(AddShortenedUrl request, CancellationToken cancellationToken)
    {
        var shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(x => x.OriginalUrl == request.Url);
        if (shortenedUrl is not null)
        {
            return new ShortenedUrlDto
            {
                ShortUrl = $"{_shorteningSettings.Url}/{shortenedUrl.ShortUrl}",
                OriginalUrl = shortenedUrl.OriginalUrl
            };
        }

        var newShortenedUrl = new Entities.ShortenedUrl
        {
            OriginalUrl = request.Url,
            ShortUrl = GenerateShortUrl()
        };

        _dbContext.ShortenedUrls.Add(newShortenedUrl);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return new ShortenedUrlDto
        {
            ShortUrl = $"{_shorteningSettings.Url}/{newShortenedUrl.ShortUrl}",
            OriginalUrl = newShortenedUrl.OriginalUrl
        };
    }

    private string GenerateShortUrl()
    {
        var random = new Random();
        var poolBuilder = new StringBuilder($"{Constants.Characters.BigChars}{Constants.Characters.SmallChars}");

        if (_shorteningSettings.Numbers) poolBuilder.Append(Constants.Characters.Numbers);
        if (_shorteningSettings.Specials) poolBuilder.Append(Constants.Characters.Specials);

        if (_shorteningSettings.Length < 1)
            throw new InvalidShorteningLengthException(_shorteningSettings.Length);

        var pool = poolBuilder.ToString();
        var result = new char[_shorteningSettings.Length];

        for (int i = 0; i < _shorteningSettings.Length; i++)
        {
            result[i] = pool[random.Next(0, pool.Length)];
        }

        return new string(result);
    }
}