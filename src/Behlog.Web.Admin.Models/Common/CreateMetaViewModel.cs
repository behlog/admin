using Behlog.Cms.Commands;

namespace Behlog.Web.Admin.Models;

public class AdminMetaViewModel : BaseViewModel
{
    public string Title { get; set; }
    public string MetaKey { get; set; }
    public string? MetaValue { get; set; }
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
}