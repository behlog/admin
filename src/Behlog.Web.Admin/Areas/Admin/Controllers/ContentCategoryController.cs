namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/ContentCategory")]
[Authorize]
public class ContentCategoryController : BaseAdminController
{
    public const string Name = "ContentCategory";
    
    public ContentCategoryController()
    {
    }
    
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
                .WithPageNumber(page).WithPageSize(10)
                .WillOrderBy("id").WillOrderDesc()
        );

        var model = await _behlog.PublishAsync(query).ConfigureAwait(false);

        return View(model);
    }

    [HttpGet("new/{langCode}/{contentTypeName}")]
    public async Task<IActionResult> New(string langCode, string contentTypeName)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();

        var model = new CreateContentCategoryViewModel
        {
            LangId = langId, 
            LangCode = langCode,
            LangTitle = FindLangTitle(langCode),
            ContentTypeId = contentType.Id,
            ContentTypeName = contentType.SystemName,
            ContentTypeTitle = contentType.Title,
            //TODO : Add support for Parent
        };

        return View(model);
    }

    [HttpPost("new/{langCode}/{contentTypeName}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> New(
        [FromRoute]string langCode,
        [FromRoute]string contentTypeName,
        [FromForm]CreateContentCategoryViewModel model)
    {
        model.ThrowExceptionIfArgumentIsNull(nameof(model));
        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : read from resources
            return View(model);
        }

        var command = new CreateContentCategoryCommand(
            model.Title, model.AltTitle, model.Slug!, model.LangId,
            model.ParentId, model.Description, model.ContentTypeId, _website.Id);

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            model.WithValidationErrors(result.Errors);
            model.ModelMessage = "خطاها را برطرف کنید"; //TODO : from resource.
            return View(model);
        }

        return RedirectToAction("Edit", new { id = result.Value.Id });
    }

    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var query = new QueryContentCategoryById(id);
        var category = await _behlog.PublishAsync(query).ConfigureAwait(false);

        if (category.Status == EntityStatus.Deleted)
        {
            //TODO : it's better to have an custom exception for soft deleted entities.
            throw new InvalidOperationException("Trying to edit an entity which has been deleted before!");
        }

        var model = UpdateContentCategoryViewModel.LoadFrom(category);
        return View(model);
    }

    [HttpPost("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, UpdateContentCategoryViewModel model)
    {
        model.ThrowExceptionIfArgumentIsNull(nameof(model));

        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : read from resource
            return View(model);
        }

        var command = new UpdateContentCategoryCommand(
            model.Id, model.Title, model.AltTitle, model.Slug, 
            model.LangId, model.ParentId, model.Description, model.Enabled);

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            model.WithValidationErrors(result.Errors);
            model.ModelMessage = "خطاها را برطرف کنید"; //TODO : from resource.
            return View(model);
        }
        
        //TODO : redirect to index?
        return View(model);
    }
    
}