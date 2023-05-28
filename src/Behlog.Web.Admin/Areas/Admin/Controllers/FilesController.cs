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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return NotFound();
    }
}