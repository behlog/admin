using Behlog.Web.Core.Localizations;

namespace Behlog.Web.Admin.Models;

using Behlog.Core.Models;

public class AdminContentIndexItemViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string? Summary { get; set; }
    public string? AltTitle { get; set; }
    public ContentStatusEnum Status { get; set; }
    public string StatusDisplay => Status.ToString(); //TODO : implement ToDisplay()
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

public class AdminContentIndexViewModel : PagedViewModel<AdminContentIndexItemViewModel>
{
    public AdminContentIndexViewModel()
    {
        Data = new List<AdminContentIndexItemViewModel>();
    }
    
    public string ContentTypeTitle { get; set; }
    public Guid ContentTypeId { get; set; }
    public string ContentTypeName { get; set; }
    public string ContentTypeSlug { get; set; }
    public Guid LangId { get; set; }
    public string LangCode { get; set; }
    public string LangTitle { get; set; }
}