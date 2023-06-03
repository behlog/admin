namespace Behlog.Web.Admin.Models;

public class UpdateFileUploadViewModel
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? AltTitle { get; set; }
    public bool Hidden { get; set; }
    public string? Url { get; set; }
    public string FileUrl { get; set; }
    public string FilePath { get; set; }
    public string? Description { get; set; }
}