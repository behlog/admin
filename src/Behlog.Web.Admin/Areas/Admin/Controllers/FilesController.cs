using Microsoft.Extensions.Logging;

namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/Files")]
[Authorize]
public class FilesController : BaseAdminController
{
    public const string Name = "Files";
    public const string Action_Index = nameof(Index);
    public const string Action_New = nameof(New);
    public const string ACTION_Edit = nameof(Edit);
    
    private readonly ILogger<FilesController> _logger;

    public FilesController()
    {
    }

    public FilesController(
        IBehlogMediator mediator, BehlogWebsite website, ILogger<FilesController> logger)
        : base(mediator, website)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("{page:int=1}")]
    public async Task<IActionResult> Index(int page = 1)
    {
        var query = new QueryFileUploadsByWebsiteId(
            _website.Id, QueryOptions.New()
                .WithPageNumber(page)
                .WithPageSize(10)
                .WillOrderBy("CreatedDate")
                .WillOrderDesc());
        
        var data = await _behlog.PublishAsync(query).ConfigureAwait(false);
        var model = new AdminFileUploadIndexViewModel
        {
            PageNumber = page,
            PageSize = query.Options.PageSize,
            Search = query.Options.Search,
            TotalCount = data.TotalCount,
            FileType = null,
            Data = data.Results.Select(_ => _.Map()).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateFileUploadViewModel model)
    {
        if (model is null) return BadRequest();

        if (model.FileData is null || model.FileData.Length == 0)
        {
            model.AddError("هیچ فایلی برای آپلود انتخاب نشد. لطفاَ‌ فایل خود را وارد کنید.", nameof(model.FileData));
            return new JsonResult(model);
        }

        var command = new CreateFileUploadCommand(
            model.FileData, "files", FileTypeEnum.Downloads, _website.Id,
            model.AlternateFileData, model.Title, model.AltTitle, model.Description);
        try
        {
            var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
            model.Succeed("آپلود فایل با موفقیت انجام گرفت.");
            return new JsonResult(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.GetBaseException().Message);
            model.AddError("خطای ناشناخته :ـ(");
            return new JsonResult(model);
        }

    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return NotFound();
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> New()
    {
        var model = new CreateFileUploadViewModel();
        return await Task.FromResult(View(model));
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> New(CreateFileUploadViewModel model)
    {
        if (model is null) return BadRequest();
        
        if (!ModelState.IsValid)
        {
            model.AddError("Model is not valid!");
            return View(model);
        }
        
        if (model.FileData is null || model.FileData.Length == 0)
        {
            model.AddError("هیچ فایلی برای آپلود انتخاب نشد. لطفاَ‌ فایل خود را وارد کنید.", nameof(model.FileData));
            return View(model);
        }

        var command = new CreateFileUploadCommand(
            model.FileData, "files", FileTypeEnum.Downloads, _website.Id,
            model.AlternateFileData, model.Title, model.AltTitle, model.Description);
        try
        {
            var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
            if (result.HasError)
            {
                AddErrorsToModel(model, result.Errors, _logger);
                return View(model);
            }
            model.Succeed("آپلود فایل با موفقیت انجام گرفت.");

            return RedirectToAction(ACTION_Edit, new {id = result.Value.Id});
        }
        catch (Exception ex)
        {
            LogError(_logger, Name, Action_New, ex);
            model.AddError("خطای ناشناخته :ـ(");
            return View(model);
        }
    }

    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var fileUpload = await _behlog.PublishAsync(
                new QueryFileUploadById(id), cancellationToken).ConfigureAwait(false);

            var model = new UpdateFileUploadViewModel
            {
                Id = fileUpload.Id,
                Title = fileUpload.Title,
                Hidden = fileUpload.Status == FileUploadStatus.Hidden,
                Description = fileUpload.Description,
                Url = fileUpload.Url,
                AltTitle = fileUpload.AltTitle,
                FilePath = fileUpload.FilePath,
                FileUrl = fileUpload.FileUrl,
                AltFileUrl = fileUpload.AltFileUrl,
                AlternateFilePath = fileUpload.AlternateFilePath,
                Extension = fileUpload.Extension,
                FileSize = fileUpload.FileSize,
                AltFileSize = fileUpload.AltFileSize,
            };
            
            return View(model);
        }
        catch (NullReferenceException)
        {
            return NotFound();
        }
    }
}