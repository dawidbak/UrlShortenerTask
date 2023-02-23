using Shouldly;
using UrlShortener.WebApp.Entities;
using UrlShortener.WebApp.Exceptions;
using UrlShortener.WebApp.Features.ShortenedUrl;
using UrlShortener.WebApp.Infrastructure;

namespace UrlShortener.WebApp.UnitTests;

public class GetShortenedUrlsHandlerTests
{
    [Fact]
    public async Task Given_GetShortenedUrls_Should_Succeed()
    {
        var getShortenedUrls = new GetShortenedUrls();
        var options = TestSettings.GetDbContextOptions();
        var context = new UrlShortenerDbContext(options);
        var handler = new GetShortenedUrlsHandler(context, TestSettings.GetShorteningSettings());

        await using (var tempContext = new UrlShortenerDbContext(options))
        {
            tempContext.ShortenedUrls.Add(new ShortenedUrl
            {
                OriginalUrl = "https://www.google.com/",
                ShortUrl = "abc12345"
            });
            await tempContext.SaveChangesAsync();
        }

        var result = await handler.Handle(getShortenedUrls, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShortenedUrls.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Given_GetShortenedUrls_Should_Return_Empty_List()
    {
        var getShortenedUrls = new GetShortenedUrls();
        var handler = new GetShortenedUrlsHandler(new UrlShortenerDbContext(TestSettings.GetDbContextOptions()),
            TestSettings.GetShorteningSettings());

        var result = await handler.Handle(getShortenedUrls, CancellationToken.None);

        result.ShortenedUrls.Count().ShouldBe(0);
    }
}