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
public class AlertsController : BaseController
{
    #region Constructor

    public AlertsController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
        IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(string itemId, UpdateItemAlerts command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.ActivateAlerts(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deaticvate(string itemId, UpdateItemAlerts command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.DeactivateAlerts(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateLowInventoryThreshold(string itemId, UpdateLowInventoryThresholdAlert command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateLowInventoryThreshold(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}