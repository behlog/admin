using Behlog.Cms.Models;
using Behlog.Core;
using Behlog.Extensions;

namespace Behlog.Web.Admin.Models;

public class UpdateContentCategoryViewModel : BaseViewModel
{
    public Guid Id { get; set; }
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
    
    /// <summary>
    /// If false, sets the Status to Disabled.
    /// </summary>
    public bool Enabled { get; set; }


    public static UpdateContentCategoryViewModel LoadFrom(ContentCategoryResult result)
    {
        result.ThrowExceptionIfArgumentIsNull(nameof(result));

        return new UpdateContentCategoryViewModel
        {
            Id = result.Id,
            Description = result.Description,
            Enabled = result.Status == EntityStatusEnum.Enabled,
            Slug = result.Slug,
            Title = result.Title,
            AltTitle = result.AltTitle,
            LangId = result.LangId,
            ParentId = result.ParentId,
            // ParentTitle = TODO : read parent title if existed,
            ContentTypeId = result.ContentTypeId.Value,
        };
    }
}