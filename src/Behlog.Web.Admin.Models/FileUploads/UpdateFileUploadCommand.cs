namespace Behlog.Web.Admin.Models;

public class UpdateFileUploadCommand
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? AltTitle { get; set; }
    public bool Hidden { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
}