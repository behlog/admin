namespace Behlog.Web.Admin.Models;

public class AdminContentFileViewModel
{
    public Guid FileId { get; set; }
    public string FileName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}