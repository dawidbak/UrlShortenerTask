using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.WebApp.Entities;

namespace UrlShortener.WebApp.Infrastructure.Configurations;

public class ShortenedUrlConfiguration : IEntityTypeConfiguration<ShortenedUrl>
{
    public void Configure(EntityTypeBuilder<ShortenedUrl> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ShortUrl);
        builder.HasIndex(x => x.OriginalUrl);
        builder.Property(x => x.OriginalUrl)
            .IsRequired();
        builder.Property(x => x.ShortUrl)
            .IsRequired();
    }
}