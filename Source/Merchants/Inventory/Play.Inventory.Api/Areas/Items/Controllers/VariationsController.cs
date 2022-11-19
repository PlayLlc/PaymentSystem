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
[Route("[area]")]
public class VariationsController : BaseController
{
    #region Constructor

    public VariationsController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
        IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]")]
    public async Task<IActionResult> CreateVariation(CreateVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.CreateVariation(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpDeleteSwagger]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}")]
    public async Task<IActionResult> RemoveVariation(string itemId, string variationId, RemoveVariation command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.RemoveVariation(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    [HttpPutSwagger]
    [ValidateAntiForgeryToken]
    [Route("{itemId}/[controller]/{variationId}/[action]")]
    public async Task<IActionResult> Name(string itemId, string variationId, UpdateItemVariationName command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateVariationName(variationId, _UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}