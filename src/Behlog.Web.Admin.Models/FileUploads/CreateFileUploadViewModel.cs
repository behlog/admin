using Microsoft.AspNetCore.Http;

namespace Behlog.Web.Admin.Models;

public class CreateFileUploadViewModel : BaseViewModel
{
    public IFormFile FileData { get; set; }
    public FileTypeEnum FileType { get; set; }
    public string? Title { get; set; }
    public IFormFile AlternateFileData { get; set; }
    public string? AltTitle { get; set; }
    public string? Description { get; set; }
    public Guid WebsiteId { get; set; }
    public string? Url { get; set; }
    
}
