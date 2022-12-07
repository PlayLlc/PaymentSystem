using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Restful.Clients;

namespace Play.Inventory.Api.Client;

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
    void CreateCategories(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> CreateCategoriesWithHttpInfo(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>List&lt;CategoryDto&gt;</returns>
    List<CategoryDto> GetAllCategories(string merchantId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>ApiResponse of List&lt;CategoryDto&gt;</returns>
    ApiResponse<List<CategoryDto>> GetAllCategoriesWithHttpInfo(string merchantId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>CategoryDto</returns>
    CategoryDto GetCategories(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>ApiResponse of CategoryDto</returns>
    ApiResponse<CategoryDto> GetCategoriesWithHttpInfo(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RemoveCategories(RemoveCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RemoveCategoriesWithHttpInfo(RemoveCategory body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task CreateCategoriesAsync(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> CreateCategoriesAsyncWithHttpInfo(CreateCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>Task of List&lt;CategoryDto&gt;</returns>
    Task<List<CategoryDto>> GetAllCategoriesAsync(string merchantId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="merchantId"> (optional)</param>
    /// <returns>Task of ApiResponse (List&lt;CategoryDto&gt;)</returns>
    Task<ApiResponse<List<CategoryDto>>> GetAllCategoriesAsyncWithHttpInfo(string merchantId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>Task of CategoryDto</returns>
    Task<CategoryDto> GetCategoriesAsync(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="categoryId"></param>
    /// <returns>Task of ApiResponse (CategoryDto)</returns>
    Task<ApiResponse<CategoryDto>> GetCategoriesAsyncWithHttpInfo(string categoryId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RemoveCategoriesAsync(RemoveCategory body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RemoveCategoriesAsyncWithHttpInfo(RemoveCategory body = null);

    #endregion Asynchronous Operations
}