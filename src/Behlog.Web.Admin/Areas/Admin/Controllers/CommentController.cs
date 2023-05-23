using Microsoft.Extensions.Logging;

namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/comment")]
[Authorize]
public class CommentController : BaseAdminController
{
    private readonly ILogger<CommentController> _logger;


    public CommentController(
        ILogger<CommentController> logger,
        IBehlogMediator behlog, BehlogWebsite website) : base(behlog, website)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}