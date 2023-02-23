using MediatR;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.WebApp.Features.ShortenedUrl.DTO;

namespace UrlShortener.WebApp.Features.ShortenedUrl;

public class ShortenedUrlController : Controller
{
    private readonly ISender _mediator;

    public ShortenedUrlController(ISender mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public IActionResult Index()
    {
        return View(new ShortenedUrlDto());
    }

    [HttpPost]
    public async Task<ActionResult<string>> Add(AddShortenedUrl command)
    {
        return View("Index", await _mediator.Send(command));
    }

    [HttpGet("{url}")]
    public async Task<IActionResult> RedirectToOriginalUrl(string url)
    {
        var original = await _mediator.Send(new GetOriginalUrl(url));
        return Redirect(original);
    }

    [HttpGet("shortened-urls")]
    public async Task<ActionResult> GetAll()
    {
        var shortenedUrls = await _mediator.Send(new GetShortenedUrls());
        return View("ShortenedUrls", shortenedUrls);
    }
}