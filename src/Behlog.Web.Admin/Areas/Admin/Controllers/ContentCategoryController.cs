namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/content/category")]
[Authorize]
public class ContentCategoryController : BaseAdminController
{
    
    public ContentCategoryController(IBehlogMediator behlog, BehlogWebsite website) 
        : base(behlog, website)
    {
    }

    [HttpGet("{langCode}/{contentTypeName}/{page:int=1}")]
    public async Task<IActionResult> Index(string langCode, string contentTypeName, int page = 1)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();


        return View();
    }
}