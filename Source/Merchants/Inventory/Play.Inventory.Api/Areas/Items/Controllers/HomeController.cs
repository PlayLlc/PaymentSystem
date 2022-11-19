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
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Api.Areas.Items.Controllers
{
    [ApiExplorerSettings(GroupName = @"Items")]
    [ApiController]
    [Area($"{nameof(Items)}")]
    [Route("[area]")]
    public class HomeController : BaseController
    {
        #region Constructor

        public HomeController(
            IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
            IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
        { }

        #endregion

        #region Instance Members

        [HttpGet]
        [ValidateAntiForgeryToken]
        [Route("{itemId}")]
        public async Task<ItemDto> Index(string itemId)
        {
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(ItemDto));

            return item.AsDto();
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IEnumerable<ItemDto>> Index([FromQuery] string merchantId, [FromQuery] int? pageSize, [FromQuery] int? position) // paging and shit
        {
            if (pageSize is null || position is null)
                return (await _ItemsRepository.GetItemsAsync(new SimpleStringId(merchantId)).ConfigureAwait(false)).Select(a => a.AsDto())
                       ?? Array.Empty<ItemDto>();

            IEnumerable<ItemDto> items =
                (await _ItemsRepository.GetItemsAsync(new SimpleStringId(merchantId), pageSize!.Value, position!.Value).ConfigureAwait(false)).Select(a =>
                    a.AsDto())
                ?? Array.Empty<ItemDto>();

            return items;
        }

        [Route("{itemId}")]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string itemId, RemoveItem command)
        {
            Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(ItemDto));

            await item.Remove(_UserRetriever, command).ConfigureAwait(false);

            return NoContent();
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

        #endregion
    }
}