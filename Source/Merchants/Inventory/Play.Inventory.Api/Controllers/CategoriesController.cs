using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CategoriesController : InventoryController
{
    #region Constructor

    public CategoriesController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository) : base(
        userRetriever, merchantsRetriever, itemsRepository, categoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpGet]
    [ValidateAntiForgeryToken]
    [Route("/Inventory/[controller]")]
    public async Task<IEnumerable<CategoryDto>> Index([FromQuery] string merchantId)
    {
        return (await _CategoryRepository.GetCategoriesAsync(new SimpleStringId(merchantId)).ConfigureAwait(false)).Select(a => a.AsDto())
               ?? Array.Empty<CategoryDto>();
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    [Route("/Inventory/[controller]/{categoryId}")]
    public async Task<CategoryDto> GetCategory(string categoryId)
    {
        Category merchantRegistration = await _CategoryRepository.GetByIdAsync(new SimpleStringId(categoryId)).ConfigureAwait(false)
                                        ?? throw new NotFoundException(typeof(Category));

        return merchantRegistration.AsDto();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategory command)
    {
        this.ValidateModel();

        Category category = await Category.CreateCategory(_UserRetriever, _MerchantsRetriever, _CategoryRepository, command).ConfigureAwait(false);

        return Created(@Url.Action("GetCategory", "Categories", new {id = category.Id})!, category.AsDto());
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(RemoveCategory command)
    {
        this.ValidateModel();

        Category category = await _CategoryRepository.GetByIdAsync(new SimpleStringId(command.CategoryId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Category));

        await category.RemoveCategory(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    #endregion
}