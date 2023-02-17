using System.Text.Json;

namespace Behlog.Web.Admin.Models;

public class AdminContentCategoryIndexViewModel
{
	public AdminContentCategoryIndexViewModel() { }
	
	public Guid ContentTypeId { get; set; }
	public string? ContentTypeName { get; set; }
	public string? ContentTypeTitle { get; set; }
	public Guid LangId { get; set; }
	public string? LangCode { get; set; }
	public string? LangTitle { get; set; }
	
	public IReadOnlyCollection<ContentCategoryTreeItemViewModel> TreeData { get; set; }
}

public static class ContentCategoryViewModelExtensions
{

	public static string ToJson(
		this IReadOnlyCollection<ContentCategoryTreeItemViewModel> items)
	{
		return JsonSerializer.Serialize(items);
	}
}