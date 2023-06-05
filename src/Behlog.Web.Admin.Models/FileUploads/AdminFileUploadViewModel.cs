using Behlog.Cms.Extensions;

namespace Behlog.Web.Admin.Models;

public class AdminFileUploadViewModel
{
    public Guid Id { get; set; }
    public Guid WebsiteId { get; set; }
    public string? Title { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public string? AlternateFilePath { get; set; }
    public string? AltFileUrl { get; set; }
    public string? Extension { get; set; }
    public long FileSize { get; set; }
    public string? AltTitle { get; set; }
    public string? Url { get; set; }
    public FileUploadStatus Status { get; set; }
    public string StatusDisplay => Status.ToDisplay();
    public FileTypeEnum FileType { get; set; }
    public string FileTypeDisplay => FileType.ToDisplay();
    public DateTime? LastStatusChangedOn { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? CreatedByUserId { get; set; }
    public string? LastUpdatedByUserId { get; set; }
    public string? CreatedByIp { get; set; }
    public string? LastUpdatedByIp { get; set; }
}

public static class AdminFileUploadViewModelMapper
{

    public static AdminFileUploadViewModel Map(this FileUploadResult _)
    {
        return new AdminFileUploadViewModel
        {
            Id = _.Id,
            Title = _.Title,
            FilePath = _.FilePath,
            FileName = _.FileName,
            AlternateFilePath = _.AlternateFilePath,
            AltTitle = _.AltTitle,
            Extension = _.Extension,
            FileSize = _.FileSize,
            Url = _.Url,
            Description = _.Description,
            Status = _.Status,
            CreatedDate = _.CreatedDate,
            FileType = _.FileType,
            LastUpdated = _.LastUpdated,
            WebsiteId = _.WebsiteId,
            CreatedByIp = _.CreatedByIp,
            CreatedByUserId = _.CreatedByUserId,
            LastStatusChangedOn = _.LastStatusChangedOn,
            LastUpdatedByIp = _.LastUpdatedByIp,
            LastUpdatedByUserId = _.LastUpdatedByUserId,
            AltFileUrl = _.AltFileUrl,
            FileUrl = _.FileUrl,
        };
    }
}