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
            _website.Id,
            QueryOptions.New().WithPageNumber(page));
        
        var data = await _behlog.PublishAsync(query).ConfigureAwait(false);

        if (data.HasError)
            return NotFound();
        
        return NotFound();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return NotFound();
    }
}