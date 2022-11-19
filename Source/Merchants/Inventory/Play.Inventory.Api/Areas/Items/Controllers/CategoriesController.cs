using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Inventory.Api.Controllers;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Areas.Items.Controllers
{
    [ApiController]
    [Area($"{nameof(Items)}")]
    [Route("[area]")]
    public class CategoriesController : InventoryController
    {
        #region Constructor

        public CategoriesController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
            IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
        { }

        #endregion

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{itemId:itemId:string}/[controller]")]
        public async Task<IActionResult> Add(string itemId, UpdateItemCategories command)
        {
            this.ValidateModel();
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

            await item.AddCategories(_UserRetriever, _CategoryRepository, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Route("{itemId:itemId:string}/[controller]")]
        public async Task<IActionResult> Remove(string itemId, UpdateItemCategories command)
        {
            this.ValidateModel();
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

            await item.RemoveCategories(_UserRetriever, command).ConfigureAwait(false);

            return NoContent();
        }

        #endregion
    }
}