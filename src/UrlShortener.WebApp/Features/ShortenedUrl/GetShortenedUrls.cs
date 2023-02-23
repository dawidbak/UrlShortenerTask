using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.WebApp.Features.ShortenedUrl.DTO;
using UrlShortener.WebApp.Infrastructure;
using UrlShortener.WebApp.Settings;

namespace UrlShortener.WebApp.Features.ShortenedUrl;

public record GetShortenedUrls() : IRequest<ShortenedUrlListDto>;

public class GetShortenedUrlsHandler : IRequestHandler<GetShortenedUrls, ShortenedUrlListDto>
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly ShorteningSettings _shorteningSettings;

    public GetShortenedUrlsHandler(UrlShortenerDbContext dbContext, IOptions<ShorteningSettings> shorteningSettings)
    {
        _dbContext = dbContext;
        _shorteningSettings = shorteningSettings.Value;
    }

    public async Task<ShortenedUrlListDto> Handle(GetShortenedUrls request, CancellationToken cancellationToken)
    {
        var shortenedUrls = await _dbContext.ShortenedUrls
            .Select(x => new { x.ShortUrl, x.OriginalUrl })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return new ShortenedUrlListDto()
        {
            ShortenedUrls = shortenedUrls.Select(x => new ShortenedUrlDto()
            {
                ShortUrl = $"{_shorteningSettings.Url}/{x.ShortUrl}",
                OriginalUrl = x.OriginalUrl
            })
        };
    }
}