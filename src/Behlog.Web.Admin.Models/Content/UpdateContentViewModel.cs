using Microsoft.AspNetCore.Http;

namespace Behlog.Web.Admin.Models;

public class UpdateContentViewModel : BaseViewModel
{
    public Guid Id { get; set; }
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
    
    public string? CoverPhotoFilePath { get; set; }
    
    public IFormFile? CoverPhotoFile { get; set; }
    
    [MaxLength(2000)]
    public string? Summary { get; set; }
    
    [MaxLength(1000)]
    public string? AltTitle { get; set; }
    
    public ContentBodyType BodyType { get; set; }
    
    public int OrderNum { get; set; }
    
    public List<AdminContentFileViewModel>? Files { get; set; }
    
    public List<AdminMetaViewModel>? Meta { get; set; }

    public SelectListViewModel? CategorySelect { get; private set; }
    
    public List<Guid>? Categories { get; set; }
    
    public SelectListViewModel? TagSelect { get; set; }

    public List<Guid>? Tags { get; set; }
    
    public void SetCategorySelect(SelectListViewModel categories)
    {
        CategorySelect = categories ?? throw new ArgumentNullException(nameof(categories));
    }

    public static UpdateContentViewModel LoadFrom(ContentResult result)
    {
        if (result is null) throw new ArgumentNullException(nameof(result));

        return new UpdateContentViewModel
        {
            Id = result.Id,
            Body = result.Body,
            Files = result.Files?.Select(_ => new AdminContentFileViewModel
            {
                Description = _.Description,
                Title = _.Title,
                FileId = _.FileId,
                FileName = _.FileName
            }).ToList() ?? new List<AdminContentFileViewModel>(),
            Slug = result.Slug,
            Summary = result.Summary,
            Meta = result.Meta?.Select(_=> _.ToViewModel()).ToList(),
            Title = result.Title,
            Tags = result.Tags?.Select(_=> _.TagId).ToList(),
            AltTitle = result.AltTitle,
            BodyType = result.BodyType,
            LangCode = result.LangCode,
            LangId = result.LangId,
            LangTitle = result.LangTitle,
            ContentTypeId = result.ContentTypeId,
            ContentTypeName = result.ContentTypeName,
            OrderNum = result.OrderNum,
            Categories = result.Categories?.Select(_=> _.CategoryId).ToList(),
            ContentTypeTitle = result.ContentTypeTitle,
            CoverPhotoFilePath = result.CoverPhoto
        };
    }
}