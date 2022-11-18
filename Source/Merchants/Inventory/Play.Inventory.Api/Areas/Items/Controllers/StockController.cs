using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Inventory.Api.Controllers;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Areas.Items.Controllers;

[ApiController]
[Area($"{nameof(Items)}")]
[Route("[area]")]
public class StockController : InventoryController
{
    #region Constructor

    public StockController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository) : base(
        userRetriever, merchantsRetriever, itemsRepository, categoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/[action]")]
    public async Task<IActionResult> AddQuantity(string itemId, AddQuantityToInventory command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.AddQuantity(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/[action]")]
    public async Task<IActionResult> RemoveQuantity(string itemId, RemoveQuantityFromInventory command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.RemoveQuantity(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/[action]")]
    public async Task<IActionResult> ActivateAlerts(string itemId, UpdateItemAlerts command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.ActivateAlerts(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/[action]")]
    public async Task<IActionResult> DeativateAlerts(string itemId, UpdateItemAlerts command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.DeactivateAlerts(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}