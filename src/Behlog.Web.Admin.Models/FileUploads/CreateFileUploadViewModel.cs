using Microsoft.AspNetCore.Http;

namespace Behlog.Web.Admin.Models;

public class CreateFileUploadViewModel : BaseViewModel
{
    [Required]
    public IFormFile FileData { get; set; }
    
    public FileTypeEnum FileType { get; set; }
    
    public string? Title { get; set; }
    public IFormFile? AlternateFileData { get; set; }
    public string? AltTitle { get; set; }
    public string? Description { get; set; }
    public Guid WebsiteId { get; set; }
    public string? Url { get; set; }
    
    public string? FileUrl { get; set; }
    public string? FilePath { get; set; }
    public string? AltFileUrl { get; set; }
    public string? AlternateFilePath { get; set; }
}
