using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UrlShortener.WebApp.Infrastructure;
using UrlShortener.WebApp.Settings;

namespace UrlShortener.WebApp.UnitTests;

internal static class TestSettings
{
    internal static DbContextOptions<UrlShortenerDbContext> GetDbContextOptions()
    {
        return new DbContextOptionsBuilder<UrlShortenerDbContext>().UseInMemoryDatabase($"test-{Guid.NewGuid()}")
            .Options;
    }

    internal static IOptions<ShorteningSettings> GetShorteningSettings()
    {
        return new OptionsWrapper<ShorteningSettings>(new ShorteningSettings()
        {
            Url = "http://localhost:5000",
            Length = 6,
            Numbers = true,
            Specials = true,
        });
    }
}