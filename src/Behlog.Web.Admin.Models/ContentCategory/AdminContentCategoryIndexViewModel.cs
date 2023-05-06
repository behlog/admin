using System.Text.Json;

namespace Behlog.Web.Admin.Models;

public class AdminContentCategoryIndexWithTreeViewModel
{
	public AdminContentCategoryIndexWithTreeViewModel() { }
	
	public Guid ContentTypeId { get; set; }
	public string? ContentTypeName { get; set; }
	public string? ContentTypeTitle { get; set; }
	public Guid LangId { get; set; }
	public string? LangCode { get; set; }
	public string? LangTitle { get; set; }
	
	public IReadOnlyCollection<ContentCategoryTreeItemViewModel> Items { get; set; }
}

public static class ContentCategoryViewModelExtensions
{

	public static string ToJson(
		this IReadOnlyCollection<ContentCategoryTreeItemViewModel> items)
	{
		return JsonSerializer.Serialize(items);
	}
}

public class AdminContentCategoryIndexViewModel
{
	public AdminContentCategoryIndexViewModel()
	{
		Items = new List<AdminContentCategoryItemViewModel>();
	}
	
	public Guid ContentTypeId { get; set; }
	public string? ContentTypeName { get; set; }
	public string? ContentTypeTitle { get; set; }
	public Guid LangId { get; set; }
	public string? LangCode { get; set; }
	public string? LangTitle { get; set; }

	public IReadOnlyCollection<AdminContentCategoryItemViewModel> Items { get; set; }
}

public class AdminContentCategoryItemViewModel
{
	public Guid Id { get; set; }
	public string Title { get; set; }
	public string? Slug { get; set; }
	public string? AltTitle { get; set; }
	public Guid? ParentId { get; set; }
	public Guid LangId { get; set; }
	public string? ParentTitle { get; set; }
}