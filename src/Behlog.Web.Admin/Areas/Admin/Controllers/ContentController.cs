using Behlog.Cms.Domain;
using Behlog.Core.Models;
using Behlog.Extensions;

namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/content")]
[Authorize]
public class ContentController : BaseAdminController
{

    public ContentController(IBehlogMediator behlog, BehlogWebsite website) 
        : base(behlog, website)
    {
    }

    [HttpGet("{langCode}/{contentTypeName}/{page:int=1}")]
    public async Task<IActionResult> Index(string langCode, string contentTypeName, int page = 1)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();
        
        var options = QueryOptions.New()
            .WithPageNumber(page)
            .WithPageSize(10)
            .WillOrderBy("id")
            .WillOrderDesc();
        
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

        var langId = BehlogSupportedLanguages.GetIdByCode(langCode);
        var contentType = await _behlog.PublishAsync(new QueryContentTypeBySystemName(
            contentTypeName, langId)).ConfigureAwait(false);
        if (contentType is null) return NotFound();

        var model = new CreateContentViewModel
        {
            LangId = langId,
            ContentTypeId = contentType.Id
        };
        
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> New(CreateContentViewModel model)
    {
        if (model is null) return BadRequest();

        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : from resource
            return View(model);
        }
        
        
        // var command = new CreateContentCommand(
        //     _website.Id, model.Title, model.Slug, model.ContentTypeId, model.LangId,
        //     model.Body, model.BodyType, model.Summary, model.AltTitle, model.OrderNum,
        //     )

        return View(model);
    }

    
}