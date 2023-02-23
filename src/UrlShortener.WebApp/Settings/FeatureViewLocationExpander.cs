using Microsoft.AspNetCore.Mvc.Razor;

namespace UrlShortener.WebApp.Settings;

public class FeatureViewLocationExpander : IViewLocationExpander
{
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        var customLocations = new List<string>
        {
            "/Features/Shared/{0}.cshtml",
            "/Features/{1}/Views/{0}.cshtml",
        };

        customLocations.AddRange(viewLocations);
        return customLocations;
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }
}