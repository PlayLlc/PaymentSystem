using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Restful.Clients;

namespace IO.Swagger;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface ICategoriesApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void CategoriesCreatePost(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> CategoriesCreatePostWithHttpInfo(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void CategoriesRemoveDelete(RemoveCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> CategoriesRemoveDeleteWithHttpInfo(RemoveCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>CategoryDto</returns>
    CategoryDto InventoryCategoriesCategoryIdGet(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>ApiResponse of CategoryDto</returns>
    ApiResponse<CategoryDto> InventoryCategoriesCategoryIdGetWithHttpInfo(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>List&lt;CategoryDto&gt;</returns>
    List<CategoryDto> InventoryCategoriesGet(string merchantId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>ApiResponse of List&lt;CategoryDto&gt;</returns>
    ApiResponse<List<CategoryDto>> InventoryCategoriesGetWithHttpInfo(string merchantId = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task CategoriesCreatePostAsync(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> CategoriesCreatePostAsyncWithHttpInfo(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task CategoriesRemoveDeleteAsync(RemoveCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> CategoriesRemoveDeleteAsyncWithHttpInfo(RemoveCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>Task of CategoryDto</returns>
    Task<CategoryDto> InventoryCategoriesCategoryIdGetAsync(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>Task of ApiResponse (CategoryDto)</returns>
    Task<ApiResponse<CategoryDto>> InventoryCategoriesCategoryIdGetAsyncWithHttpInfo(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>Task of List&lt;CategoryDto&gt;</returns>
    Task<List<CategoryDto>> InventoryCategoriesGetAsync(string merchantId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>Task of ApiResponse (List&lt;CategoryDto&gt;)</returns>
    Task<ApiResponse<List<CategoryDto>>> InventoryCategoriesGetAsyncWithHttpInfo(string merchantId = null);

    #endregion Asynchronous Operations
}