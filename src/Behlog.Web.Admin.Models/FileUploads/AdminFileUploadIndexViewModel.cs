namespace Behlog.Web.Admin.Models;

public class AdminFileUploadIndexViewModel : PagedViewModel<AdminFileUploadViewModel>
{
    public AdminFileUploadIndexViewModel()
    {
        Data = new List<AdminFileUploadViewModel>();
    }
    
    public string? Search { get; set; }
    public FileTypeEnum? FileType { get; set; }
}