using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/content")]
[Authorize]
public class ContentController : BaseAdminController
{
    private ILogger<ContentController> _logger;
    public const string Name = "Content";
    public const string Action_Index = nameof(Index);
    public const string Action_New = nameof(New);
    public const string Action_Edit = nameof(Edit);
    public const string ACTION_Delete = nameof(SoftDelete);
    
    private readonly IAdminContentViewModelProvider _contentViewModelProvider;
    
    public ContentController(
        ILogger<ContentController> logger,
        IBehlogMediator behlog, BehlogWebsite website, 
        IAdminContentViewModelProvider contentViewModelProvider) 
        : base(behlog, website)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            .WillOrderBy("CreatedDate").WillOrderDesc();
        
        var query = new QueryContentByWebsiteAndContentType(
            _website.Id, langId, contentType.Id, null, options);
        var result = await _behlog.PublishAsync(query).ConfigureAwait(false);
        var model = new AdminContentIndexViewModel
        {
            LangId = langId,
            LangCode = langCode,
            LangTitle = FindLangTitle(langCode),
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            ContentTypeId = contentType.Id,
            ContentTypeName = contentType.SystemName,
            ContentTypeSlug = contentType.Slug,
            TotalCount = result.TotalCount,
            ContentTypeTitle = contentType.Title,
            Data = result.Results.Select(_=> new AdminContentIndexItemViewModel
            {
                Id = _.Id,
                Slug = _.Slug,
                Summary = _.Summary,
                Title = _.Title,
                AltTitle = _.AltTitle,
                Status = _.Status,
                ContentTypeId = _.ContentTypeId,
                CreatedDate = _.CreatedDate,
                IconName = _.IconName,
                LangId = _.LangId,
                LastUpdated = _.LastUpdated,
                LikesCount = _.LikesCount,
                OrderNum = _.OrderNum,
                AuthorUserId = _.AuthorUserId,
                AuthorUserName = _.AuthorUserName,
                ContentTypeName = _.ContentTypeName,
                ContentTypeTitle = _.ContentTypeTitle,
                AuthorUserDisplayName = _.AuthorUserDisplayName
            }).ToList()
        };
        
        return View(model);
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

        Console.WriteLine(model.PublishDateValue);
        Console.WriteLine(model.PublishTimeValue);

        if (!ModelState.IsValid)
        {
            if (_isDevelopment)
            {
                model.AddError(LogModelState(ModelState));
            }
            
            model.AddError("");//TODO : from resource
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
            if (_isDevelopment)
            {
                model.AddError(LogModelState(ModelState));
            }
            
            model.AddError(""); //TODO : from resource
            await _contentViewModelProvider.LoadUpdateViewModelAsync(model).ConfigureAwait(false);
            return View(model);
        }

        var command = new UpdateContentCommand(model.Id)
            .WithTitle(model.Title)
            .WithAltTitle(model.AltTitle)
            .WithSummary(model.Summary)
            .WithSlug(model.Slug)
            .WithBodyType(model.BodyType)
            .WithBody(model.Body)
            .WithCategories(model.Categories?.ToList())
            .WithFiles(model.Files?.Select(f => f.ToCommand()).ToList())
            .WithMeta(model.Meta?.Select(m => m.ToCommand()).ToList())
            .WithContentTypeName(model.ContentTypeName)
            .WithCoverPhotoFile(model.CoverPhotoFile)
            .Build();

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            await _contentViewModelProvider.LoadUpdateViewModelAsync(model).ConfigureAwait(false);
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
    
    private string LogModelState(ModelStateDictionary modelState)
    {
        if(modelState is null) throw new ArgumentNullException(nameof(modelState));
        
        var errors = modelState.Values.SelectMany(v => v.Errors);
        string error_messages = "";
        foreach (var error in errors)
        {
            error_messages += error.ErrorMessage + Environment.NewLine;
        }
        
        _logger.LogDebug(error_messages);
        return error_messages;
    }
}