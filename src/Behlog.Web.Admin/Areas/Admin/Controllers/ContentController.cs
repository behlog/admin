using Behlog.Core.Models;
using Behlog.Extensions;

namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/content")]
[Authorize]
public class ContentController : Controller
{
    private readonly IBehlogMediator _behlog;
    private readonly BehlogWebsite _website;

    
    public ContentController(IBehlogMediator behlog, BehlogWebsite website)
    {
        _behlog = behlog ?? throw new ArgumentNullException(nameof(behlog));
        _website = website ?? throw new ArgumentNullException(nameof(website));
    }

    [HttpGet("{langCode}/{contentTypeName}/{page:int=1}")]
    public async Task<IActionResult> Index(string langCode, string contentTypeName, int page = 1)
    {
        var langId = BehlogSupportedLanguages.GetIdByCode(langCode);
        var contentType = await _behlog.PublishAsync(
            new QueryContentTypeBySystemName(contentTypeName, langId)).ConfigureAwait(false);

        var options = new QueryOptions
        {
            OrderBy = "id",
            OrderDesc = true,
            PageNumber = page,
            PageSize = 10
        };
        
        var query = new QueryContentByWebsiteAndContentType(
            _website.Id, langId, contentType.Id, null, options);
        var result = await _behlog.PublishAsync(query).ConfigureAwait(false);

        return View(result);
    }


    [HttpGet("{langCode}/{contentTypeName}")]
    public async Task<IActionResult> New(string langCode, string contentTypeName)
    {
        if (langCode.IsNullOrEmptySpace()) return BadRequest();
        if (contentTypeName.IsNullOrEmptySpace()) return BadRequest();

        return View();
    }
    
    
    
}