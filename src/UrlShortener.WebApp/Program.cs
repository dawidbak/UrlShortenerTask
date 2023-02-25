using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using UrlShortener.WebApp.Infrastructure;
using UrlShortener.WebApp.Settings;
using Serilog;


// For Rider's problems with views locations
[assembly: AspMvcViewLocationFormat("/Features/{1}/Views/{0}.cshtml"),
           AspMvcViewLocationFormat("/Features/Shared/{0}.cshtml")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// fluent validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// In-Memory database
builder.Services.AddDbContext<UrlShortenerDbContext>(options => options.UseInMemoryDatabase("UrlShortenerDb"));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// views location
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Features/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationFormats.Add("/Features/Shared/{0}" + RazorViewEngine.ViewExtension);
    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
});

// settings
builder.Services.Configure<ShorteningSettings>(
    builder.Configuration.GetSection(ShorteningSettings.SectionName));

// serilog 
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ShortenedUrl}/{action=Index}/{id?}");

app.Run();