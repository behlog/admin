namespace Behlog.Web.Admin.Controllers;


public abstract class BaseAdminController : Controller
{
    protected readonly IBehlogMediator _behlog;
    protected readonly BehlogWebsite _website;


    public BaseAdminController(IBehlogMediator behlog, BehlogWebsite website)
    {
        _behlog = behlog ?? throw new ArgumentNullException(nameof(behlog));
        _website = website ?? throw new ArgumentNullException(nameof(website));
    }


    protected Guid FindLangId(string langCode)
    {
        if (langCode.IsNullOrEmptySpace()) 
            throw new ArgumentNullException(nameof(langCode));
        
        return BehlogSupportedLanguages.GetIdByCode(langCode);
    }


    protected async Task<ContentTypeResult> FindContentTypeAsync(Guid langId, string contentTypeName)
    {
        if (contentTypeName.IsNullOrEmptySpace())
            throw new ArgumentNullException(nameof(contentTypeName));
        
        return await _behlog.PublishAsync(
            new QueryContentTypeBySystemName(contentTypeName, langId)).ConfigureAwait(false);
    }
}