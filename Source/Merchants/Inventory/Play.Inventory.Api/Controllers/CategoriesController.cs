using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Api.Controllers;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Commands.CategoriPlay.Inventory.Contracts.Commands.Categoriessing
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

PlayPlay.Inventory.Contracts.Dtosy.InventoPlay.Inventory.Domain
using PPlay.Inventory.Domain.Aggregatess;
usingPlay.Inventory.Domain.Repositoriesusing PlaPlay.Inventory.Domain.Services Play.InvPlay.Mvc.Extensionsers;

[ApiController]
[Route("[controller]/[action]")]
public class CategoriesController : InventoryController
{
    #region Constructor

    public CategoriesController(
        IRetrieveUsers userRetriever, IR
        chants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository) : base(
        userRetriever, merchantsRetrieve
        pository, categoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpGet]
    [ValidateAntiForgeryToken]
    [Route("/Inventory/[controller]")]
    public async Task<IEnumerable<CategoryDto>> Index([FromQuery] string merchantId)
    {
        return (await _CategoryRepository.GetCategoriesAsync(new SimpleStringId(merchantId)).ConfigureAwait(false)).Select(a => a.AsDto())
               ?? Array.Empty<CategoryDt
                  [HttpGet]
    [ValidateAntiForgeryToken]
    [Route("/Inventory/[controller]/{categoryId}")]
    public async Task<CategoryDto> GetCategory(string categoryId)
    {
        Category merchantRegistration = await _CategoryRepository.GetByIdAsync(new SimpleStringId(categoryId)).ConfigureAwait(false)
                                        
                                        gory));

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

        var category = await _CategoryRepository.GetByIdAsync(new SimpleStringId(command.CategoryId)).ConfigureAwait(false)
                       ?? throw new NotF
                       gory));

        await category.RemoveCategory(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    #endregion
}