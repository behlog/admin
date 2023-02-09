namespace Behlog.Web.Admin.Models;

public class AdminContentCategoryIndexViewModel
{
	public AdminContentCategoryIndexViewModel() { }

	public AdminContentCategoryIndexViewModel(QueryResult<ContentCategoryResult> data) 
	{
		data.ThrowExceptionIfArgumentIsNull(nameof(data));
		Data = data;
	}

	public Guid ContentTypeId { get; set; }
	public string? ContentTypeName { get; set; }
	public string? ContentTypeTitle { get; set; }
	public Guid LangId { get; set; }
	public string? LangCode { get; set; }
	public string? LangTitle { get; set; }

    public QueryResult<ContentCategoryResult> Data { get; set; }
}