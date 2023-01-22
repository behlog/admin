namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]")]
[Authorize]
public class HomeController : Controller
{
    
    
    public HomeController()
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}