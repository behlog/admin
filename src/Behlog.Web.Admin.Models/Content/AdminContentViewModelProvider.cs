using Behlog.Cms;
using Behlog.Core;
using Behlog.Cms.Query;
using Behlog.Web.Admin.Models;
using Behlog.Web.Services.Contracts;

namespace Behlog.Web.Admin.ViewModelProviders;


public interface IAdminContentViewModelProvider
{
    
    /// <summary>
    /// Returns a New <see cref="CreateContentViewModel"/> and load it's needed relations data.
    /// </summary>
    /// <param name="langCode"></param>
    /// <param name="contentTypeName"></param>
    Task<CreateContentViewModel> NewCreateViewModelAsync(string langCode, string contentTypeName);

    /// <summary>
    /// Provides <see cref="UpdateContentViewModel"/> with the data loaded from <see cref="ContentResult"/>
    /// </summary>
    Task<UpdateContentViewModel> LoadUpdateViewModelAsync(ContentResult content);
}

public class AdminContentViewModelProvider : IAdminContentViewModelProvider
{
    private readonly IBehlogMediator _behlog;
    private readonly IContentCategoryProvider _contentCategoryProvider;
    
    public AdminContentViewModelProvider(
        IBehlogMediator behlog, IContentCategoryProvider contentCategoryProvider)
    {
        _behlog = behlog ?? throw new ArgumentNullException(nameof(behlog));
        _contentCategoryProvider = contentCategoryProvider 
                                   ?? throw new ArgumentNullException(nameof(contentCategoryProvider));
    }

    /// <inheritdoc /> 
    public async Task<CreateContentViewModel> NewCreateViewModelAsync(string langCode, string contentTypeName)
    {
        if (langCode.IsNullOrEmpty()) throw new ArgumentNullException(nameof(langCode));
        if (contentTypeName.IsNullOrEmpty()) throw new ArgumentNullException(nameof(contentTypeName));

        var langId = BehlogSupportedLanguages.GetIdByCode(langCode);
        var contentType = await _behlog.PublishAsync(
            new QueryContentTypeBySystemName(contentTypeName, langId)).ConfigureAwait(false);

        contentType.ThrowExceptionIfReferenceIsNull($"The ContentType '{contentTypeName}' not found.");

        var model = new CreateContentViewModel
        {
            LangId = langId,
            LangCode = langCode,
            LangTitle = BehlogSupportedLanguages.GetTitleById(langId),
            ContentTypeId = contentType.Id,
            ContentTypeName = contentTypeName,
            ContentTypeTitle = contentType.Title
        };
        
        model.SetCategorySelect(await _contentCategoryProvider
            .GetSelectListAsync(langId, contentTypeName).ConfigureAwait(false));

        return await Task.FromResult(model);
    }

    /// <inheritdoc /> 
    public async Task<UpdateContentViewModel> LoadUpdateViewModelAsync(ContentResult content)
    {
        content.ThrowExceptionIfArgumentIsNull(nameof(content));
        
        var model = UpdateContentViewModel.LoadFrom(content);
        model.SetCategorySelect(await _contentCategoryProvider
            .GetSelectListAsync(content.LangId, content.ContentTypeName!).ConfigureAwait(false));

        return await Task.FromResult(model);
    }
}
