namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/content")]
[Authorize]
public class ContentController : BaseAdminController
{
    public const string Name = "Content";
    public const string Action_Index = nameof(Index);
    public const string Action_New = nameof(New);
    public const string Action_Edit = nameof(Edit);
    
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
            .NewCreateViewModelAsync(langCode, contentTypeName).ConfigureAwait(false);

        return View(model);
    }

    [HttpPost("new/{langCode}/{contentTypeName}")]
    public async Task<IActionResult> New(
        string langCode, string contentTypeName, CreateContentViewModel model)
    {
        if (model is null) return BadRequest();

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            string error_messages = "";
            foreach (var error in errors)
            {
                error_messages += error.ErrorMessage + Environment.NewLine;
            }
            model.AddError(error_messages); //TODO : from resource

            await _contentViewModelProvider.LoadCreateViewModelAsync(model).ConfigureAwait(false);
            return View(model);
        }

        var command = new CreateContentCommand(
                _website.Id, model.Title, model.Slug!, model.ContentTypeId, model.LangId,
                model.Body!, model.BodyType, model.Summary!, model.AltTitle!, model.OrderNum,
                model.Categories,
                model.Meta?.Select(_ => _.ToCommand()).ToList()!,
                model.Files?.Select(_ => _.ToCommand())!,
                model.Tags)
            .WithContentTypeName(model.ContentTypeName!)
            .WithPassword(model.Password!)
            .WithCoverPhotoFile(model.CoverPhotoFile!);

        if (model.PublishDate.HasValue)
        {
            command.WillBePublishedOn(model.PublishDate.Value);
        }

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            model.WithValidationErrors(result.Errors);
            model.ModelMessage = "خطاها را برطرف کنید"; //TODO : from resource.
            return View(model);
        }

        return RedirectToAction(Action_Edit, new { id = result.Value.Id });
    }

    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var content = await _behlog.PublishAsync(new QueryContentById(id)).ConfigureAwait(false);
        if (content is null) return NotFound();

        var model = await _contentViewModelProvider
            .LoadUpdateViewModelAsync(content).ConfigureAwait(false);
        
        return View(model);
    }

    [HttpPost("edit/{id:guid}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateContentViewModel model)
    {
        if (model is null) return BadRequest();

        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : from resource
            return View(model);
        }

        var command = new UpdateContentCommand(model.Id)
        {
            Body = model.Body!,
            Password = model.Password,
            Slug = model.Slug,
            Summary = model.Summary,
            Title = model.Title,
            AltTitle = model.AltTitle,
            BodyType = model.BodyType,
            // IconName = model.icon TODO : add iconName
            LangId = model.LangId,
            // IsDraft = model.isd
            OrderNum = model.OrderNum,
            // ViewPath = model.view
            PublishDate = model.PublishDate,
            ContentTypeId = model.ContentTypeId,
            Categories = model.Categories?.ToList(),
            Files = model.Files?.Select(_=> _.ToCommand()).ToList(),
            Meta = model.Meta?.Select(_=> _.ToCommand()).ToList(),
            ContentTypeName = model.ContentTypeName,
            CoverPhotoFile = model.CoverPhotoFile,
            Tags = model.Tags?.ToList(),
        };

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            model.WithValidationErrors(result.Errors);
            model.ModelMessage = "خطاها را برطرف کنید"; //TODO : from resource.
            return View(model);
        }
        
        return View(model);
    }

    [HttpPost("delete/{id:guid}")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        var command = new SoftDeleteContentCommand(id);
        try
        {
            await _behlog.PublishAsync(command).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] : Cannot SoftDelete Content with Id : '{id}' ");
            Console.WriteLine($"Exception : {ex.GetBaseException().Message}");
            // return new JsonResult(new
            // {
            //     success = false, message = "cannot be soft deleted"
            // });
            throw;
        }

        return new JsonResult(new
        {
            success = true
        });
    }
    
    
}