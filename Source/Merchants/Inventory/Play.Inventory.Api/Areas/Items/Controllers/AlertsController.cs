using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Inventory.Api.Controllers;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;
using Play.Mvc.Extensions;

using System.Runtime.CompilerServices;

using Play.Mvc.Attributes;

namespace Play.Inventory.Api.Areas.Items.Controllers;

[ApiController]
[Area($"{nameof(Items)}")]
public class AlertsController : BaseController
{
    #region Constructor

    public AlertsController(
        IRetrieveUsers userRetriever, IRetrieveMerchants merchantsRetriever, IItemRepository itemsRepository, ICategoryRepository categoryRepository,
        IInventoryRepository inventoryRepository) : base(userRetriever, merchantsRetriever, itemsRepository, categoryRepository, inventoryRepository)
    { }

    #endregion

    #region Instance Members

    [HttpPutSwagger(template: "/Inventory/[area]/{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ActivateAlerts(string itemId, UpdateItemAlerts command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.ActivateAlerts(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "/Inventory/[area]/{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeaticvateAlerts(string itemId, UpdateItemAlerts command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(itemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.DeactivateAlerts(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    [HttpPutSwagger(template: "/Inventory/[area]/{itemId}/[controller]/[action]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateLowInventoryThreshold(string itemId, UpdateLowInventoryThresholdAlert command)
    {
        this.ValidateModel();
        Item item = await _ItemsRepository.GetByIdAsync(new SimpleStringId(command.ItemId)).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Item));

        await item.UpdateLowInventoryThreshold(_UserRetriever, command).ConfigureAwait(false);

        return Ok();
    }

    #endregion
}