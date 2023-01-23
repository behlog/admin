using Behlog.Core.Models;

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

        var query = new QueryContentCategoriesFiltered(
            _website.Id, langId, contentType.Id, null,
            QueryOptions.New()
                .WithPageNumber(page)
                .WithPageSize(10)
                .WillOrderBy("id")
                .WillOrderDesc()
        );

        var model = await _behlog.PublishAsync(query).ConfigureAwait(false);

        return View(model);
    }

    [HttpGet("{langCode}/{contentTypeName}")]
    public async Task<IActionResult> New(string langCode, string contentTypeName)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();

        var model = new CreateContentCategoryViewModel
        {
            LangId = langId, ContentTypeId = contentType.Id
        };

        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> New(CreateContentCategoryViewModel model)
    {
        model.ThrowExceptionIfArgumentIsNull(nameof(model));
        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : read from resources
            return View(model);
        }

        var command = new CreateContentCategoryCommand(
            model.Title, model.AltTitle, model.Slug, model.LangId,
            model.ParentId, model.Description, model.ContentTypeId, _website.Id);

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            foreach (var err in result.Errors)
            {
                model.AddError(err.Message ,err.FieldName);
            }

            return View(model);
        }
        
        //TODO : redirects to Edit mode

        return View(model);
    }
}