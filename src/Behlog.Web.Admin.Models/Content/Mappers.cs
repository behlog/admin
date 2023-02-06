using Behlog.Cms.Commands;

namespace Behlog.Web.Admin.Models;

public static class ContentMappers
{

    public static ContentFileCommand ToCommand(this AdminContentFileViewModel model)
    {
        if (model is null) throw new ArgumentNullException(nameof(model));

        return new ContentFileCommand(
            model.FileId, model.FileName, model.Title, model.Description);
    }
}