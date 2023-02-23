using Shouldly;
using UrlShortener.WebApp.Features.ShortenedUrl;
using UrlShortener.WebApp.Exceptions;
using UrlShortener.WebApp.Infrastructure;

namespace UrlShortener.WebApp.UnitTests;

public class AddShortenedUrlHandlerTests
{
    [Fact]
    public async Task Given_AddShortenedUrl_With_Wrong_Length_Should_Fail()
    {
        var addShortenedUrl = new AddShortenedUrl("https://www.google.com/");
        var tempSettings = TestSettings.GetShorteningSettings();
        tempSettings.Value.Length = 0;
        var handler = new AddShortenedUrlHandler(new UrlShortenerDbContext(TestSettings.GetDbContextOptions()), tempSettings);

        var result = await Record.ExceptionAsync(() => handler.Handle(addShortenedUrl, CancellationToken.None));

        result.ShouldNotBeNull();
        result.ShouldBeOfType<InvalidShorteningLengthException>();
    }

    [Fact]
    public async Task Given_AddShortenedUrl_Should_Succeed()
    {
        var addShortenedUrl = new AddShortenedUrl("https://www.google.com/");
        var handler = new AddShortenedUrlHandler(new UrlShortenerDbContext(TestSettings.GetDbContextOptions()),
            TestSettings.GetShorteningSettings());

        var result = await handler.Handle(addShortenedUrl, CancellationToken.None);

        result.ShouldNotBeNull();
        result.OriginalUrl.ShouldBe(addShortenedUrl.Url);
        result.ShortUrl.ShouldNotBeNullOrEmpty();
    }
}