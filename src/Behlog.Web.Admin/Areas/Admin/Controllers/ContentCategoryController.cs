namespace Behlog.Web.Admin.Controllers;

[Area(WebsiteAreaNames.Admin)]
[Route("[area]/ContentCategory")]
[Authorize]
public class ContentCategoryController : BaseAdminController
{
    public const string Name = "ContentCategory";
    
    public ContentCategoryController()
    {
    }
    
    public ContentCategoryController(IBehlogMediator behlog, BehlogWebsite website) 
        : base(behlog, website)
    {
    }

    [HttpGet("{langCode}/{contentTypeName}/{page:int=1}")]
    public async Task<IActionResult> Index(string langCode, string contentTypeName, int page = 1)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();

        var model = new AdminContentCategoryIndexWithTreeViewModel
        {
            LangCode = langCode,
            LangTitle = FindLangTitle(langCode),
            LangId = langId,
            ContentTypeId = contentType.Id,
            ContentTypeName = contentType.SystemName,
            ContentTypeTitle = contentType.Title,
            Items = await LoadTreeAsync(langId)
        };
        
        return View(model);
    }

    [HttpGet("new/{langCode}/{contentTypeName}")]
    public async Task<IActionResult> New(string langCode, string contentTypeName)
    {
        var langId = FindLangId(langCode);
        var contentType = await FindContentTypeAsync(langId, contentTypeName);
        if (contentType is null) return NotFound();

        var model = new CreateContentCategoryViewModel
        {
            LangId = langId, 
            LangCode = langCode,
            LangTitle = FindLangTitle(langCode),
            ContentTypeId = contentType.Id,
            ContentTypeName = contentType.SystemName,
            ContentTypeTitle = contentType.Title,
            //TODO : Add support for Parent
        };

        return View(model);
    }

    [HttpPost("new/{langCode}/{contentTypeName}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> New(
        [FromRoute]string langCode,
        [FromRoute]string contentTypeName,
        [FromForm]CreateContentCategoryViewModel model)
    {
        model.ThrowExceptionIfArgumentIsNull(nameof(model));
        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : read from resources
            return View(model);
        }

        var command = new CreateContentCategoryCommand(
            model.Title, model.AltTitle, model.Slug!, model.LangId,
            model.ParentId, model.Description, model.ContentTypeId, _website.Id);

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            model.WithValidationErrors(result.Errors);
            model.ModelMessage = "خطاها را برطرف کنید"; //TODO : from resource.
            return View(model);
        }

        return RedirectToAction("Edit", new { id = result.Value.Id });
    }

    [HttpGet("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        var query = new QueryContentCategoryById(id);
        var category = await _behlog.PublishAsync(query).ConfigureAwait(false);

        if (category.Status == EntityStatus.Deleted)
        {
            //TODO : it's better to have an custom exception for soft deleted entities.
            throw new InvalidOperationException("Trying to edit an entity which has been deleted before!");
        }

        var model = UpdateContentCategoryViewModel.LoadFrom(category);
        return View(model);
    }

    [HttpPost("edit/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, UpdateContentCategoryViewModel model)
    {
        model.ThrowExceptionIfArgumentIsNull(nameof(model));

        if (!ModelState.IsValid)
        {
            model.AddError(""); //TODO : read from resource
            return View(model);
        }

        var command = new UpdateContentCategoryCommand(
            model.Id, model.Title, model.AltTitle!, model.Slug!, 
            model.LangId, model.ParentId, model.Description!, model.Enabled);

        var result = await _behlog.PublishAsync(command).ConfigureAwait(false);
        if (result.HasError)
        {
            model.WithValidationErrors(result.Errors);
            model.ModelMessage = "خطاها را برطرف کنید"; //TODO : from resource.
            return View(model);
        }
        
        //TODO : redirect to index?
        return View(model);
    }


    #region private helpers

    private async Task<IReadOnlyCollection<ContentCategoryTreeItemViewModel>> LoadTreeAsync(Guid langId)
    {
        var query = new QueryContentCategoryByParentId(langId, parentId: null);
        var rootCats = await _behlog.PublishAsync(query).ConfigureAwait(false);
        var result = new List<ContentCategoryTreeItemViewModel>();
        foreach (var root in rootCats)
        {
            var rootNode = GetTreeItem(root);
            Console.WriteLine($"adding nodes for {root.Title}");
            await AddNodes(rootNode, langId);
            result.Add(rootNode);
        }

        return result;
    }
    
    // private async Task<ContentCategoryTreeViewModel> GetTreeAsync() {
    //     var query = new QueryContentCategoryByParentId(null);
    //     var rootCats = await _behlog.PublishAsync(query).ConfigureAwait(false);
    //     var items = new List<ContentCategoryTreeItemViewModel>();
    //     foreach(var root in rootCats)
    //     {
    //         await AddNodes(GetTreeItem(root));
    //     }
    //
    //     return new ContentCategoryTreeViewModel(items);
    // }

    private async Task AddNodes(ContentCategoryTreeItemViewModel parent, Guid langId)
    {
        var query = new QueryContentCategoryByParentId(langId, parent.Id);
        var childs = await _behlog.PublishAsync(query).ConfigureAwait(false);

        foreach (var child in childs)
        {
            var node = GetTreeItem(child);
            Console.WriteLine($"adding nodes for {child.Title}");
            parent.AddNode(node);
            await AddNodes(node, langId);
        }
    }

    // private async Task<IReadOnlyCollection<ContentCategoryTreeItemViewModel>> GetByParentId(Guid? parentId) {
    //     var query = new QueryContentCategoryByParentId(parentId);
    //     var cats = await _behlog.PublishAsync(query).ConfigureAwait(false);
    //     
    //     if(cats is null || !cats.Any()) {
    //         return new List<ContentCategoryTreeItemViewModel>();
    //     }
    //
    //     var result = new List<ContentCategoryTreeItemViewModel>();
    //
    //     foreach(var cat in cats) {
    //         result.Add(GetTreeItem(cat));
    //
    //         var childs = await GetByParentId(cat.Id);
    //         if (childs.Any()) 
    //             result.AddRange(childs);
    //     }
    //
    //     return result;
    // }

    private ContentCategoryTreeItemViewModel GetTreeItem(ContentCategoryResult category) {
        category.ThrowExceptionIfArgumentIsNull(nameof(category));
        return new ContentCategoryTreeItemViewModel(
            category.Id, category.ParentId, category.Title,
            $"item_{category.Id}", new[] { category.Id.ToString() });
    }

    #endregion
}