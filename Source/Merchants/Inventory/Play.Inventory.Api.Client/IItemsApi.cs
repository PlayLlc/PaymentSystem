using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Commands.Items;
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
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>List&lt;ItemDto&gt;</returns>
    List<ItemDto> ItemsGet(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>ApiResponse of List&lt;ItemDto&gt;</returns>
    ApiResponse<List<ItemDto>> ItemsGetWithHttpInfo(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdCategoriesDelete(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdCategoriesDeleteWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdCategoriesPost(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdCategoriesPostWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdDescriptionPut(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdDescriptionPutWithHttpInfo(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdLocationsDelete(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdLocationsDeleteWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdLocationsPost(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdLocationsPostWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdLocationsSetAllPut(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdLocationsSetAllPutWithHttpInfo(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdNamePut(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdNamePutWithHttpInfo(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdPricePut(string itemId, UpdateItemPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdPricePutWithHttpInfo(string itemId, UpdateItemPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdSkuPut(string itemId, UpdateItemSku body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdSkuPutWithHttpInfo(string itemId, UpdateItemSku body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdStockActivateAlertsPut(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdStockActivateAlertsPutWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdStockAddQuantityPut(string itemId, AddQuantityToInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdStockAddQuantityPutWithHttpInfo(string itemId, AddQuantityToInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdStockDeativateAlertsPut(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdStockDeativateAlertsPutWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdStockRemoveQuantityPut(string itemId, RemoveQuantityFromInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdStockRemoveQuantityPutWithHttpInfo(string itemId, RemoveQuantityFromInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsPost(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsPostWithHttpInfo(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsVariationIdAddQuantityPut(string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsVariationIdAddQuantityPutWithHttpInfo(
        string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsVariationIdDelete(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsVariationIdDeleteWithHttpInfo(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsVariationIdNamePut(string itemId, string variationId, UpdateItemVariationName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsVariationIdNamePutWithHttpInfo(string itemId, string variationId, UpdateItemVariationName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsVariationIdPricePut(string itemId, string variationId, UpdateItemVariationPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsVariationIdPricePutWithHttpInfo(string itemId, string variationId, UpdateItemVariationPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsVariationIdRemoveQuantityPut(string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsVariationIdRemoveQuantityPutWithHttpInfo(
        string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ItemsItemIdVariationsVariationIdSkuPut(string itemId, string variationId, UpdateItemVariationSku body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ItemsItemIdVariationsVariationIdSkuPutWithHttpInfo(string itemId, string variationId, UpdateItemVariationSku body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>Task of List&lt;ItemDto&gt;</returns>
    Task<List<ItemDto>> ItemsGetAsync(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <param name="pageSize"> (optional)</param>
    /// <param name="position"> (optional)</param>
    /// <returns>Task of ApiResponse (List&lt;ItemDto&gt;)</returns>
    Task<ApiResponse<List<ItemDto>>> ItemsGetAsyncWithHttpInfo(string merchantId = null, int? pageSize = null, int? position = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdCategoriesDeleteAsync(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdCategoriesDeleteAsyncWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdCategoriesPostAsync(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdCategoriesPostAsyncWithHttpInfo(string itemId, UpdateItemCategories body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdDescriptionPutAsync(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdDescriptionPutAsyncWithHttpInfo(string itemId, UpdateItemDescription body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdLocationsDeleteAsync(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdLocationsDeleteAsyncWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdLocationsPostAsync(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdLocationsPostAsyncWithHttpInfo(string itemId, UpdateItemLocations body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdLocationsSetAllPutAsync(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdLocationsSetAllPutAsyncWithHttpInfo(string itemId, SetAllLocationsForItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdNamePutAsync(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdNamePutAsyncWithHttpInfo(string itemId, UpdateItemName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdPricePutAsync(string itemId, UpdateItemPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdPricePutAsyncWithHttpInfo(string itemId, UpdateItemPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdSkuPutAsync(string itemId, UpdateItemSku body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdSkuPutAsyncWithHttpInfo(string itemId, UpdateItemSku body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdStockActivateAlertsPutAsync(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdStockActivateAlertsPutAsyncWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdStockAddQuantityPutAsync(string itemId, AddQuantityToInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdStockAddQuantityPutAsyncWithHttpInfo(string itemId, AddQuantityToInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdStockDeativateAlertsPutAsync(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdStockDeativateAlertsPutAsyncWithHttpInfo(string itemId, UpdateItemAlerts body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdStockRemoveQuantityPutAsync(string itemId, RemoveQuantityFromInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdStockRemoveQuantityPutAsyncWithHttpInfo(string itemId, RemoveQuantityFromInventory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsPostAsync(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsPostAsyncWithHttpInfo(string itemId, CreateVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsVariationIdAddQuantityPutAsync(string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsVariationIdAddQuantityPutAsyncWithHttpInfo(
        string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsVariationIdDeleteAsync(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsVariationIdDeleteAsyncWithHttpInfo(string itemId, string variationId, RemoveVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsVariationIdNamePutAsync(string itemId, string variationId, UpdateItemVariationName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsVariationIdNamePutAsyncWithHttpInfo(string itemId, string variationId, UpdateItemVariationName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsVariationIdPricePutAsync(string itemId, string variationId, UpdateItemVariationPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsVariationIdPricePutAsyncWithHttpInfo(
        string itemId, string variationId, UpdateItemVariationPrice body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsVariationIdRemoveQuantityPutAsync(string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsVariationIdRemoveQuantityPutAsyncWithHttpInfo(
        string itemId, string variationId, UpdateQuantityToInventoryForVariation body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ItemsItemIdVariationsVariationIdSkuPutAsync(string itemId, string variationId, UpdateItemVariationSku body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="itemId"></param>
    /// <param name="variationId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ItemsItemIdVariationsVariationIdSkuPutAsyncWithHttpInfo(string itemId, string variationId, UpdateItemVariationSku body = null);

    #endregion Asynchronous Operations
}