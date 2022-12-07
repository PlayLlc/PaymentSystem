using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Restful.Clients;

namespace Play.Inventory.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IItemsApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsActivateAlerts(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsActivateAlertsWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsAddCategories(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsAddCategoriesWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsAddLocations(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsAddLocationsWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsCreateVariations(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsCreateVariationsWithHttpInfo(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsDeaticvateAlerts(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsDeaticvateAlertsWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>List&lt;ItemDto&gt;</returns>
    List<ItemDto> ItemsGetAllItems(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>ApiResponse of List&lt;ItemDto&gt;</returns>
    ApiResponse<List<ItemDto>> ItemsGetAllItemsWithHttpInfo(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <returns>ItemDto</returns>
    ItemDto ItemsGetItems(string itemId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <returns>ApiResponse of ItemDto</returns>
    ApiResponse<ItemDto> ItemsGetItemsWithHttpInfo(string itemId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsRemoveCategories(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsRemoveCategoriesWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsRemoveItems(string itemId, RemoveItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsRemoveItemsWithHttpInfo(string itemId, RemoveItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsRemoveLocations(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsRemoveLocationsWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsRemoveVariations(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsRemoveVariationsWithHttpInfo(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsSetAllLocations(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsSetAllLocationsWithHttpInfo(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsUpdateDescriptionItems(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsUpdateDescriptionItemsWithHttpInfo(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsUpdateLowInventoryThresholdAlerts(string itemId, UpdateLowInventoryThresholdAlert body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsUpdateLowInventoryThresholdAlertsWithHttpInfo(string itemId, UpdateLowInventoryThresholdAlert body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsUpdateNameItems(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsUpdateNameItemsWithHttpInfo(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsUpdateNameVariations(string itemId, string variationId, UpdateItemVariationName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsUpdateNameVariationsWithHttpInfo(string itemId, string variationId, UpdateItemVariationName body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsActivateAlertsAsync(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsActivateAlertsAsyncWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsAddCategoriesAsync(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsAddCategoriesAsyncWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsAddLocationsAsync(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsAddLocationsAsyncWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsCreateVariationsAsync(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsCreateVariationsAsyncWithHttpInfo(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsDeaticvateAlertsAsync(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsDeaticvateAlertsAsyncWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>Task of List&lt;ItemDto&gt;</returns>
    Task<List<ItemDto>> ItemsGetAllItemsAsync(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>Task of ApiResponse (List&lt;ItemDto&gt;)</returns>
    Task<ApiResponse<List<ItemDto>>> ItemsGetAllItemsAsyncWithHttpInfo(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <returns>Task of ItemDto</returns>
    Task<ItemDto> ItemsGetItemsAsync(string itemId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <returns>Task of ApiResponse (ItemDto)</returns>
    Task<ApiResponse<ItemDto>> ItemsGetItemsAsyncWithHttpInfo(string itemId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsRemoveCategoriesAsync(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsRemoveCategoriesAsyncWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsRemoveItemsAsync(string itemId, RemoveItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsRemoveItemsAsyncWithHttpInfo(string itemId, RemoveItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsRemoveLocationsAsync(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsRemoveLocationsAsyncWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsRemoveVariationsAsync(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsRemoveVariationsAsyncWithHttpInfo(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsSetAllLocationsAsync(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsSetAllLocationsAsyncWithHttpInfo(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsUpdateDescriptionItemsAsync(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsUpdateDescriptionItemsAsyncWithHttpInfo(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsUpdateLowInventoryThresholdAlertsAsync(string itemId, UpdateLowInventoryThresholdAlert body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsUpdateLowInventoryThresholdAlertsAsyncWithHttpInfo(string itemId, UpdateLowInventoryThresholdAlert body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsUpdateNameItemsAsync(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsUpdateNameItemsAsyncWithHttpInfo(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsUpdateNameVariationsAsync(string itemId, string variationId, UpdateItemVariationName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsUpdateNameVariationsAsyncWithHttpInfo(string itemId, string variationId, UpdateItemVariationName body = null);

    #endregion Asynchronous Operations
}