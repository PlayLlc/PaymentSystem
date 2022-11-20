using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Api.Controllers;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Areas.Items.Controllers;

[ApiController]
[Area($"{nameof(Items)}")]
public class VariationsController : BaseController
{
    #region Constructor

    public VariationsController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
        IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPostSwagger(template: "/Inventory/[area]/{itemId}/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.CreateVariation(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpDeleteSwagger(template: "/Inventory/[area]/{itemId}/[controller]/{variationId}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string itemId, string variationId, RemoveVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.RemoveVariation(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    [HttpPutSwagger(template: "/Inventory/[area]/{itemId}/[controller]/{variationId}/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateName(string itemId, string variationId, UpdateItemVariationName command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateVariationName(variationId, _UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}