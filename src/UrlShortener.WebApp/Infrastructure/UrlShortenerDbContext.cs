using Microsoft.EntityFrameworkCore;
using UrlShortener.WebApp.Entities;

namespace UrlShortener.WebApp.Infrastructure;

public class UrlShortenerDbContext : DbContext
{
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
    {
    }

    public DbSet<ShortenedUrl> ShortenedUrls => Set<ShortenedUrl>();
}