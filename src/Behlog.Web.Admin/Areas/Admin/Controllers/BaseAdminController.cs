using System.Security.Policy;
using Behlog.Core.Validations;
using Microsoft.Extensions.Logging;

namespace Behlog.Web.Admin.Controllers;


public abstract class BaseAdminController : Controller
{
    protected readonly IBehlogMediator _behlog;
    protected readonly BehlogWebsite _website;
    protected bool _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    public BaseAdminController()
    {
    }

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

    protected string FindLangTitle(string langCode)
    {
        if (langCode.IsNullOrEmptySpace()) 
            throw new ArgumentNullException(nameof(langCode));

        return BehlogSupportedLanguages.GetTitleByCode(langCode);
    }
    
    protected async Task<ContentTypeResult> FindContentTypeAsync(Guid langId, string contentTypeName)
    {
        if (contentTypeName.IsNullOrEmptySpace())
            throw new ArgumentNullException(nameof(contentTypeName));
        
        return await _behlog.PublishAsync(
            new QueryContentTypeBySystemName(contentTypeName, langId)).ConfigureAwait(false);
    }

    protected void AddErrorsToModel(
        BaseViewModel model, IReadOnlyCollection<ValidationError> errors, ILogger logger)
    {
        if (model is null) throw new ArgumentNullException(nameof(model));
        if(errors is null || !errors.Any()) return;

        foreach (var err in errors)
        {
            model.AddError(err.Message, err.FieldName);
            if (_isDevelopment && err.Exception != null)
            {
                LogError(logger, err.Exception, err.ErrorCode, err.FieldName);
            }
        }
    }

    protected void LogError(
        ILogger logger, Exception exception, string errorCode = null, string fieldName = null)
    {
        if (logger is null) throw new ArgumentNullException(nameof(logger));
        if (exception is null) throw new ArgumentNullException(nameof(exception));
        
        logger.LogError($"{errorCode}: [{fieldName}]: {exception.GetBaseException().Message}");
    }

    protected void LogError(
        ILogger logger, string controller, string action, Exception exception)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        string message = $"{DateTime.UtcNow.ToLocalTime():G} {controller}.{action}: {exception.GetBaseException().Message}";
        logger.LogError(message);
    }
}