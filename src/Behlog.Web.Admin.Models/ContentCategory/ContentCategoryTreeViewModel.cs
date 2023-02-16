using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Behlog.Web.Admin;

public class ContentCategoryTreeViewModel
{
    public ContentCategoryTreeViewModel() { 
        Items = new List<ContentCategoryTreeItemViewModel>();
    }

    public ContentCategoryTreeViewModel(
        IReadOnlyCollection<ContentCategoryTreeItemViewModel> items) 
    {
        Items = items;
    }

    public IReadOnlyCollection<ContentCategoryTreeItemViewModel> Items { get; set; }
}

public class ContentCategoryTreeItemViewModel {

    public ContentCategoryTreeItemViewModel() { }

    public ContentCategoryTreeItemViewModel(
        Guid id, Guid? parentId, string text, string href, string[] tags) 
    {
        this.Id = id;
        this.ParentId = parentId;
        this.Text = text;
        this.href = href;
        this.Tags = tags;
    }

    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public string Text { get; set; }
    public string href { get; set; }
    public string[] Tags { get; set; }
}

