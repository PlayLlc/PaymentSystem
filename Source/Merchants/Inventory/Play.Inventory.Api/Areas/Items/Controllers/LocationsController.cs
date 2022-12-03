using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Api.Controllers;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Inventory.Api.Areas.Items.Controllers;

[ApiController]
[Area($"{nameof(Items)}")]
[Route("/Inventory/[area]")]
public class LocationsController : BaseController
{
    #region Constructor

    public LocationsController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
        IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetAll(string itemId, SetAllLocationsForItem command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.SetAllLocations(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPostSwagger(template: "{itemId}/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(string itemId, UpdateItemLocations command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.AddStore(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpDeleteSwagger(template: "{itemId}/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(string itemId, UpdateItemLocations command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.RemoveStore(_UserRetriever, command).ConfigureAwait(false);

        return NoContent();
    }

    #endregion
}