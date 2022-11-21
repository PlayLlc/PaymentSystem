using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Restful.Clients;

namespace Play.Identity.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IMerchantApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void AddressMerchant(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> AddressMerchantWithHttpInfo(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void BusinessInfoMerchant(UpdateMerchantBusinessInfo body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> BusinessInfoMerchantWithHttpInfo(UpdateMerchantBusinessInfo body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void CompanyNameMerchant(UpdateMerchantCompanyName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> CompanyNameMerchantWithHttpInfo(UpdateMerchantCompanyName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>MerchantDto</returns>
    MerchantDto GetMerchant(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of MerchantDto</returns>
    ApiResponse<MerchantDto> GetMerchantWithHttpInfo(string id = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task AddressMerchantAsync(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> AddressMerchantAsyncWithHttpInfo(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task BusinessInfoMerchantAsync(UpdateMerchantBusinessInfo body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> BusinessInfoMerchantAsyncWithHttpInfo(UpdateMerchantBusinessInfo body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task CompanyNameMerchantAsync(UpdateMerchantCompanyName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> CompanyNameMerchantAsyncWithHttpInfo(UpdateMerchantCompanyName body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of MerchantDto</returns>
    Task<MerchantDto> GetMerchantAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse (MerchantDto)</returns>
    Task<ApiResponse<MerchantDto>> GetMerchantAsyncWithHttpInfo(string id = null);

    #endregion Asynchronous Operations
}