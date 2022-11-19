using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Inventory.Domain;
using Play.Inventory.Contracts.Commands;
using Play.Mvc.Extensions;
using Play.Inventory.Api.Controllers;

namespace Play.Inventory.Api.Areas.Items.Controllers
{
    [ApiController]
    [Area($"{nameof(Items)}")]
    [Route("[area]")]
    public class HomeController : InventoryController
    {
        #region Constructor

        public HomeController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository) :
            base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository)
        { }

        #endregion

        #region Instance Members

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IEnumerable<ItemDto>> Index([FromQuery] string merchantId, [FromQuery] int? pageSize, [FromQuery] int? position) // paging and shit
        {
            if (pageSize is null || position is null)
                return await _ItemsRepository.GetItemsAsync(new SimpleStringId(merchantId)).ConfigureAwait(false) ?? Array.Empty<ItemDto>();

            var items = await _ItemsRepository.GetItemsAsync(new SimpleStringId(merchantId), pageSize!.Value, position!.Value).ConfigureAwait(false)
                        ?? Array.Empty<ItemDto>();

            return items;
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("{itemId}/[action]")]
        public async Task<IActionResult> Description(string itemId, UpdateItemDescription command)
        {
            this.ValidateModel();
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

            await item.UpdateDescription(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("{itemId}/[action]")]
        public async Task<IActionResult> Name(string itemId, UpdateItemName command)
        {
            this.ValidateModel();
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

            await item.UpdateName(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("{itemId}/[action]")]
        public async Task<IActionResult> Price(string itemId, UpdateItemPrice command)
        {
            this.ValidateModel();
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

            await item.UpdatePrice(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        [Route("{itemId}/[action]")]
        public async Task<IActionResult> Sku(string itemId, UpdateItemSku command)
        {
            this.ValidateModel();
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

            await item.UpdateSku(_UserRetriever, command).ConfigureAwait(false);

            return Ok();
        }

        #endregion
    }
}