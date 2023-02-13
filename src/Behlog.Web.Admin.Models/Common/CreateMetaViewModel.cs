using Behlog.Cms.Commands;
using Behlog.Core;

namespace Behlog.Web.Admin.Models;

public class AdminMetaViewModel : BaseViewModel
{
    public string Title { get; set; }
    public string MetaKey { get; set; }
    public string? MetaValue { get; set; }
    public string? MetaType { get; set; }
    public string? Category { get; set; }
    public int OrderNum { get; set; }
    public string? Description { get; set; }
    public bool Enabled { get; set; }
    public Guid? LangId { get; set; }
}


public static class MetaMappers
{

    public static MetaCommand ToCommand(this AdminMetaViewModel model)
    {
        if (model is null) throw new ArgumentNullException(nameof(model));

        return new MetaCommand
        {
            Category = model.Category,
            Description = model.Description,
            Enabled = model.Enabled,
            Title = model.Title,
            LangId = model.LangId,
            MetaKey = model.MetaKey,
            MetaValue = model.MetaValue,
            OrderNum = model.OrderNum,
            OwnerId = default
        };
    }

    public static AdminMetaViewModel ToViewModel(this MetaResult meta)
    {
        return new AdminMetaViewModel
        {
            Category = meta.Category,
            LangId = meta.LangId,
            Description = meta.Description,
            Enabled = meta.Status == EntityStatus.Enabled,
            Title = meta.Title,
            MetaKey = meta.MetaKey,
            MetaValue = meta.MetaValue,
            OrderNum = meta.OrderNum,
            MetaType = meta.MetaType
        };
    }
}