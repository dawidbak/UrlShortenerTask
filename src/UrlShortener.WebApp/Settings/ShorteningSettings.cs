namespace UrlShortener.WebApp.Settings;

public class ShorteningSettings
{
    public static string SectionName => "Shortening";

    public string Url { get; set; } = default!;
    public bool Numbers { get; set; }
    public bool Specials { get; set; }
    public int Length { get; set; }
}