using Play.Inventory.Contracts.Dtos;
using Play.Restful.Clients;

namespace Play.Inventory.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IInventoryApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>InventoryDto</returns>
    InventoryDto GetInventory(string storeId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>ApiResponse of InventoryDto</returns>
    ApiResponse<InventoryDto> GetInventoryWithHttpInfo(string storeId);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>Task of InventoryDto</returns>
    Task<InventoryDto> GetInventoryAsync(string storeId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>Task of ApiResponse (InventoryDto)</returns>
    Task<ApiResponse<InventoryDto>> GetInventoryAsyncWithHttpInfo(string storeId);

    #endregion Asynchronous Operations
}