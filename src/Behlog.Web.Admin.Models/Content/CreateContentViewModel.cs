using Behlog.Cms.Domain;

namespace Behlog.Web.Admin.Models;


public class CreateContentViewModel : BaseViewModel
{
    public Guid LangId { get; set; }
    public Guid ContentTypeId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }
    
    [MaxLength(1000)]
    public string Slug { get; set; }
    
    public string Body { get; set; }
    
    public string CoverPhoto { get; set; }
    
    [MaxLength(2000)]
    public string Summary { get; set; }
    
    [MaxLength(1000)]
    public string AltTitle { get; set; }
    
    public ContentBodyTypeEnum BodyType { get; set; }
    
    public int OrderNum { get; set; }
}