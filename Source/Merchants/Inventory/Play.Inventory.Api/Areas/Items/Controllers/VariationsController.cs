using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
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
public class VariationsController : InventoryController
{
    #region Constructor

    public VariationsController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository) : base(
        userRetriever, merchantsRetriever, itemsRepository, categoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]")]
    public async Task<IActionResult> CreateVariation(CreateVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.CreateVariation(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpDelete]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}")]
    public async Task<IActionResult> RemoveVariation(string itemId, string variationId, RemoveVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.RemoveVariation(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}/[action]")]
    public async Task<IActionResult> Name(string itemId, string variationId, UpdateItemVariationName command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateVariationName(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}/[action]")]
    public async Task<IActionResult> Price(string itemId, string variationId, UpdateItemVariationPrice command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateVariationPrice(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}/[action]")]
    public async Task<IActionResult> Sku(string itemId, string variationId, UpdateItemVariationSku command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateVariationSku(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}/[action]")]
    public async Task<IActionResult> AddQuantity(string itemId, string variationId, UpdateQuantityToInventoryForVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.AddQuantity(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPut]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}/[action]")]
    public async Task<IActionResult> RemoveQuantity(string itemId, string variationId, UpdateQuantityToInventoryForVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.RemoveQuantity(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}