using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Identity.Contracts.Dtos;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CategoriesController : InventoryController
{
    #region Instance Values

    private readonly ILogger<CategoriesController> _Logger;

    #endregion

    #region Constructor

    public CategoriesController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
        ILogger<CategoriesController> logger) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository)
    {
        _Logger = logger;
    }

    #endregion

    #region Instance Members

    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<CategoryDto> Index([FromQuery] string id)
    {
        Category merchantRegistration = await _CategoryRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(Category));

        return merchantRegistration.AsDto();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategory command)
    {
        this.ValidateModel();

        Category category = await Category.CreateCategory(_UserRetriever, _MerchantsRetriever, _CategoryRepository, command).ConfigureAwait(false);

        return Created(@Url.Action("Index", "Categories", new {id = category.Id})!, category.AsDto());
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(RemoveCategory command)
    {
        this.ValidateModel();

        var category = await _CategoryRepository.GetByIdAsync(new SimpleStringId(command.CategoryId)).ConfigureAwait(false)
                       ?? throw new NotFoundException(typeof(Category));

        await category.RemoveCategory(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    #endregion
}