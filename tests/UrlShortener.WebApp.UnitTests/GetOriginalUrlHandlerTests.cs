using Shouldly;
using UrlShortener.WebApp.Entities;
using UrlShortener.WebApp.Exceptions;
using UrlShortener.WebApp.Features.ShortenedUrl;
using UrlShortener.WebApp.Infrastructure;

namespace UrlShortener.WebApp.UnitTests;

public class GetOriginalUrlHandlerTests
{
    [Fact]
    public async Task Given_GetOriginalUrl_Should_Succeed()
    {
        var getOriginalUrl = new GetOriginalUrl("abcd12345");
        var options = TestSettings.GetDbContextOptions();
        var context = new UrlShortenerDbContext(options);
        var handler = new GetOriginalUrlHandler(context);

        await using (var tempContext = new UrlShortenerDbContext(options))
        {
            tempContext.ShortenedUrls.Add(new ShortenedUrl
            {
                OriginalUrl = "https://www.google.com/",
                ShortUrl = getOriginalUrl.Url
            });
            await tempContext.SaveChangesAsync();
        }

        var result = await handler.Handle(getOriginalUrl, CancellationToken.None);

        result.ShouldNotBeNull();
        result.ShouldBe("https://www.google.com/");
    }
    
    [Fact]
    public async Task Given_GetOriginalUrl_Should_Not_Exists()
    {
        var getOriginalUrl = new GetOriginalUrl("abcd12345");
        var handler = new GetOriginalUrlHandler(new UrlShortenerDbContext(TestSettings.GetDbContextOptions()));

        var result = await Record.ExceptionAsync(() => handler.Handle(getOriginalUrl, CancellationToken.None));

        result.ShouldNotBeNull();
        result.ShouldBeOfType<ShortenedUrlNotFoundException>();
    }
}