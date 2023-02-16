namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]")]
[Authorize]
public class HomeController : Controller
{
    public const string Name = "Home";
    public const string ActionIndex = nameof(Index);

    public HomeController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}