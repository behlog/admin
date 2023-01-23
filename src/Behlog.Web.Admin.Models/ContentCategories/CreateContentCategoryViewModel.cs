namespace Behlog.Web.Admin.Models;

public class CreateContentCategoryViewModel : BaseViewModel
{
    public Guid LangId { get; set; }
    public Guid ContentTypeId { get; set; }
    public Guid? ParentId { get; set; }
    
    [Required]
    [MaxLength(256)]
    public string Title { get; set; }
    
    [MaxLength(500)]
    public string? AltTitle { get; set; }
    
    [MaxLength(256)]
    public string? Slug { get; set; }
    
    [MaxLength()]
    public string? Description { get; set; }
    
    /// <summary>
    /// Just for displaying the Parent Title if has any.
    /// </summary>
    public string? ParentTitle { get; set; }
}