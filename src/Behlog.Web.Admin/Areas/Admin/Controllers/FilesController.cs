namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/Files")]
[Authorize]
public class FilesController : BaseAdminController
{
    public const string Name = "Files";

    public FilesController()
    {
    }

    public FilesController(IBehlogMediator mediator, BehlogWebsite website)
        : base(mediator, website)
    {
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
            model.FileData, FileTypeEnum.Downloads, _website.Id,
            model.AlternateFileData, model.Title, model.AltTitle, model.Description);
        try
        {
            var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
            model.Succeed("آپلود فایل با موفقیت انجام گرفت.");
            return new JsonResult(model);
        }
        catch (Exception ex)
        {
            model.AddError("خطای ناشناخته :ـ(");
            return new JsonResult(model);
        }

    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return NotFound();
    }
}