namespace Behlog.Web.Admin.Models;

public class CreateContentCategoryViewModel : BaseViewModel
{
    public CreateContentCategoryViewModel() { }

    public CreateContentCategoryViewModel(SelectListViewModel languageSelect)
    {
        SetLanguages(languageSelect);
    }

    public CreateContentCategoryViewModel(
        SelectListViewModel languageSelect, SelectListViewModel contentTypeSelect)
    {
        SetLanguages(languageSelect);
        SetContentTypes(contentTypeSelect);
    }

    public Guid LangId { get; set; }
    public string? LangTitle { get; set; }
    public string? LangCode { get; set; }
    public Guid ContentTypeId { get; set; }
    public string? ContentTypeName { get; set; }
    public string? ContentTypeTitle { get; set; }
    public Guid? ParentId { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Title { get; set; }
    
    [MaxLength(500)]
    public string? AltTitle { get; set; }
    
    [MaxLength(256)]
    public string? Slug { get; set; }
    
    [MaxLength(2000)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Just for displaying the Parent Title if has any.
    /// </summary>
    public string? ParentTitle { get; set; }
    
    
    public SelectListViewModel? LanguageSelect { get; private set; }
    
    public SelectListViewModel? ContentTypeSelect { get; private set; }


    public void SetLanguages(SelectListViewModel model)
    {
        LanguageSelect = model ?? throw new ArgumentNullException(nameof(model));
    }

    public void SetContentTypes(SelectListViewModel model)
    {
        ContentTypeSelect = model ?? throw new ArgumentNullException(nameof(model));
    }
}