using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Behlog.Web.Admin.Models;


public class CreateContentViewModel : BaseViewModel
{
    public CreateContentViewModel()
    {
        Files = new List<AdminContentFileViewModel>();
        Meta = new List<AdminMetaViewModel>();
    }
    
    public Guid LangId { get; set; }
    public string? LangCode { get; set; }
    public string? LangTitle { get; set; }
    public Guid ContentTypeId { get; set; }
    public string? ContentTypeName { get; set; }
    public string? ContentTypeTitle { get; set; }
    

    [Required]
    [MaxLength(1000)]
    public string Title { get; set; }
    
    [MaxLength(1000)]
    public string? Slug { get; set; }
    
    public string? Body { get; set; }
    
    public IFormFile? CoverPhotoFile { get; set; }
    
    [MaxLength(2000)]
    public string? Summary { get; set; }
    
    [MaxLength(1000)]
    public string? AltTitle { get; set; }
    
    public ContentBodyType BodyType { get; set; }
    
    public int OrderNum { get; set; }
    
    public string? Password { get; set; }
    
    public DateTime? PublishDate { get; set; }
    public string? PublishDateValue { get; set; }
    public string? PublishTimeValue { get; set; }
    
    public List<AdminContentFileViewModel> Files { get; set; }
    
    public List<AdminMetaViewModel>? Meta { get; set; }

    public IEnumerable<SelectListItem>? CategorySelect { get; private set; }
    
    public List<Guid>? Categories { get; set; }
    
    public List<Guid>? Tags { get; set; }
    
    public IEnumerable<SelectListItem>? TagSelect { get; private set; }

    public void SetCategorySelect(SelectListViewModel categories)
    {
        CategorySelect = categories?.Items
            .Select(_=> new SelectListItem(_.Text, _.Value)) 
                         ?? throw new ArgumentNullException(nameof(categories));
    }

    public void SetTagSelect(SelectListViewModel tags)
    {
        TagSelect = tags?.Items
            .Select(_=> new SelectListItem(_.Text, _.Value))
                        ?? throw new ArgumentNullException(nameof(tags));
    }
}