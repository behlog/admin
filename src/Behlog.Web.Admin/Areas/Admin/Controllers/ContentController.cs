namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/content")]
[Authorize]
public class ContentController : BaseAdminController
{
    private readonly IAdminContentViewModelProvider _contentViewModelProvider;
    
    public ContentController(
        IBehlogMediator behlog, BehlogWebsite website, 
        IAdminContentViewModelProvider contentViewModelProvider) 
        : base(behlog, website)
    {
        _contentViewModelProvider = contentViewModelProvider 
                                    ?? throw new ArgumentNullException(nameof(contentViewModelProvider));
    }

    [HttpGet("{langCode}/{contentTypeName}/{page:int=1}")]
    public async Task<IActionResult> Index(string langCode, string contentTypeName, int page = 1)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();
        
        var options = QueryOptions.New()
            .WithPageNumber(page).WithPageSize(10)
            .WillOrderBy("id").WillOrderDesc();
        
        var query = new QueryContentByWebsiteAndContentType(
            _website.Id, langId, contentType.Id, null, options);
        var result = await _behlog.PublishAsync(query).ConfigureAwait(false);

        return View(result);
    }


    [HttpGet("new/{langCode}/{contentTypeName}")]
    public async Task<IActionResult> New(string langCode, string contentTypeName)
    {
        if (langCode.IsNullOrEmptySpace()) return BadRequest();
        if (contentTypeName.IsNullOrEmptySpace()) return BadRequest();

        var model = await _contentViewModelProvider
            .NewCreateViewModel(langCode, contentTypeName).ConfigureAwait(false);

        return View(model);
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