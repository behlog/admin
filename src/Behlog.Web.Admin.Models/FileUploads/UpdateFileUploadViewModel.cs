using Microsoft.AspNetCore.Http;

namespace Behlog.Web.Admin.Models;

public class UpdateFileUploadViewModel : BaseViewModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? AltTitle { get; set; }
    public IFormFile? AlternateFileData { get; set; }
    public bool Hidden { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    
    public string? FileUrl { get; set; }
    public string? FilePath { get; set; }
    public string? AltFileUrl { get; set; }
    public string? AlternateFilePath { get; set; }
    public long FileSize { get; set; }
    public string? Extension { get; set; }
    public long? AltFileSize { get; set; }
}