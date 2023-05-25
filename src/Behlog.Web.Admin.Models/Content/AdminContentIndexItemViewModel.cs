using Behlog.Web.Core.Localizations;

namespace Behlog.Web.Admin.Models;

public class AdminContentIndexItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Summary { get; set; }
    public string? AltTitle { get; set; }
    public ContentStatusEnum Status { get; set; }
    public string StatusDisplay => Status.ToDisplay();
    public int OrderNum { get; set; }
    public Guid LangId { get; set; }
    public Guid ContentTypeId { get; set; }
    public string? ContentTypeName { get; set; }
    public string? ContentTypeTitle { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreateDateDisplay => CreatedDate.ToLocalTime().ToFriendlyPersianDateTime();
    public DateTime? LastUpdated { get; set; }
    public string? LastUpdatedDisplay => LastUpdated?.ToLocalTime().ToFriendlyPersianDateTime();
    public string? IconName { get; set; }
    public string AuthorUserId { get; set; }
    public string? AuthorUserName { get; set; }
    public string? AuthorUserDisplayName { get; set; }
    public int LikesCount { get; set; }
}

public static class AdminContentIndexItemViewModelMapper
{

    public static AdminContentIndexItemViewModel Map(this ContentResult _)
        => new AdminContentIndexItemViewModel
        {
            Id = _.Id,
            Slug = _.Slug,
            Summary = _.Summary,
            Title = _.Title,
            AltTitle = _.AltTitle,
            Status = _.Status,
            ContentTypeId = _.ContentTypeId,
            CreatedDate = _.CreatedDate,
            IconName = _.IconName,
            LangId = _.LangId,
            LastUpdated = _.LastUpdated,
            LikesCount = _.LikesCount,
            OrderNum = _.OrderNum,
            AuthorUserId = _.AuthorUserId,
            AuthorUserName = _.AuthorUserName,
            ContentTypeName = _.ContentTypeName,
            ContentTypeTitle = _.ContentTypeTitle,
            AuthorUserDisplayName = _.AuthorUserDisplayName
        };

}