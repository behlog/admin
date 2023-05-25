namespace Behlog.Web.Admin.Models;

using Behlog.Core.Models;

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