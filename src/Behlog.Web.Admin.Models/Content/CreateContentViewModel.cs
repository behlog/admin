namespace Behlog.Web.Admin.Models;


public class CreateContentViewModel : BaseViewModel
{
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
    
    
}