namespace Behlog.Web.Admin.Models;

public class AdminFileUploadViewModel
{
    public Guid Id { get; set; }
    public Guid WebsiteId { get; set; }
    public string? Title { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string? AlternateFilePath { get; set; }
    public string? Extension { get; set; }
    public long FileSize { get; set; }
    public string? AltTitle { get; set; }
    public string? Url { get; set; }
    public FileUploadStatus Status { get; set; }
    public string StatusDisplay => Status.ToString(); //TODO: Must read from resources
    public FileTypeEnum FileType { get; set; }
    public string FileTypeDisplay => FileType.ToString(); //TODO: must read from resource
    public DateTime? LastStatusChangedOn { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? CreatedByUserId { get; set; }
    public string? LastUpdatedByUserId { get; set; }
    public string? CreatedByIp { get; set; }
    public string? LastUpdatedByIp { get; set; }
}