using System.Net;

using Play.Domain.Exceptions;
using Play.Inventory.Api.Client;
using Play.Inventory.Contracts.Dtos;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Services;
using Play.Restful.Clients;

namespace Play.Loyalty.Application.Services;

public class InventoryItemRetriever : IRetrieveInventoryItems
{
    #region Instance Values

    private readonly IItemsApi _ItemsApi;

    #endregion

    #region Constructor

    public InventoryItemRetriever(IItemsApi itemsApi)
    {
        _ItemsApi = itemsApi;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ApiException"></exception>
    public async Task<InventoryItem> GetByIdAsync(string itemId, string variationId)
    {
        try
        {
            ItemDto dto = await _ItemsApi.ItemsGetItemsAsync(itemId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(InventoryDto));
            VariationDto variation = dto.Variations.FirstOrDefault(a => a.Id == variationId) ?? throw new NotFoundException(typeof(InventoryDto));

            return new(variation);
        }

        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, e);
        }
    }

    #endregion
}