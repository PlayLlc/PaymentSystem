using Play.Identity.Contracts.Commands.MerchantRegistration;
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
    void ApprovePost(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ApprovePostWithHttpInfo(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>MerchantRegistrationDto</returns>
    MerchantRegistrationDto Get(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of MerchantRegistrationDto</returns>
    ApiResponse<MerchantRegistrationDto> GetWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void Post(CreateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> PostWithHttpInfo(CreateMerchantRegistrationCommand body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ApprovePostAsync(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ApprovePostAsyncWithHttpInfo(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of MerchantRegistrationDto</returns>
    Task<MerchantRegistrationDto> GetAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse (MerchantRegistrationDto)</returns>
    Task<ApiResponse<MerchantRegistrationDto>> GetAsyncWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task PostAsync(CreateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> PostAsyncWithHttpInfo(CreateMerchantRegistrationCommand body = null);

    #endregion Asynchronous Operations
}