using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Commands.MerchantRegistration;
using Play.Identity.Contracts.Dtos;
using Play.Restful.Clients;

namespace Play.Identity.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IRegistrationApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationApproveMerchant(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationApproveMerchantWithHttpInfo(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns></returns>
    void RegistrationApproveUser(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationApproveUserWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="userRegistrationId"> (optional)</param>
    /// <returns></returns>
    void RegistrationCompleteRegistration(string userRegistrationId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="userRegistrationId"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationCompleteRegistrationWithHttpInfo(string userRegistrationId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationCreateMerchant(CreateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationCreateMerchantWithHttpInfo(CreateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationCreateUser(CreateUserRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationCreateUserWithHttpInfo(CreateUserRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns></returns>
    void RegistrationGetEmailVerificationForUser(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationGetEmailVerificationForUserWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>MerchantRegistrationDto</returns>
    MerchantRegistrationDto RegistrationGetMerchant(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of MerchantRegistrationDto</returns>
    ApiResponse<MerchantRegistrationDto> RegistrationGetMerchantWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns></returns>
    void RegistrationGetPhoneVerificationForUser(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationGetPhoneVerificationForUserWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>UserRegistrationDto</returns>
    UserRegistrationDto RegistrationGetUser(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>ApiResponse of UserRegistrationDto</returns>
    ApiResponse<UserRegistrationDto> RegistrationGetUserWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationUpdateAddressForUser(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationUpdateAddressForUserWithHttpInfo(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationUpdateContactForUser(UpdateContactCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationUpdateContactForUserWithHttpInfo(UpdateContactCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationUpdateEmailVerificationForUser(VerifyConfirmationCodeCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationUpdateEmailVerificationForUserWithHttpInfo(VerifyConfirmationCodeCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationUpdatePersonalDetailsForUser(UpdatePersonalDetailCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationUpdatePersonalDetailsForUserWithHttpInfo(UpdatePersonalDetailCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void RegistrationUpdatePhoneVerificationForUser(VerifyConfirmationCodeCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> RegistrationUpdatePhoneVerificationForUserWithHttpInfo(VerifyConfirmationCodeCommand body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationApproveMerchantAsync(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationApproveMerchantAsyncWithHttpInfo(UpdateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationApproveUserAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationApproveUserAsyncWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="userRegistrationId"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationCompleteRegistrationAsync(string userRegistrationId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="userRegistrationId"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationCompleteRegistrationAsyncWithHttpInfo(string userRegistrationId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationCreateMerchantAsync(CreateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationCreateMerchantAsyncWithHttpInfo(CreateMerchantRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationCreateUserAsync(CreateUserRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationCreateUserAsyncWithHttpInfo(CreateUserRegistrationCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationGetEmailVerificationForUserAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationGetEmailVerificationForUserAsyncWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of MerchantRegistrationDto</returns>
    Task<MerchantRegistrationDto> RegistrationGetMerchantAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse (MerchantRegistrationDto)</returns>
    Task<ApiResponse<MerchantRegistrationDto>> RegistrationGetMerchantAsyncWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationGetPhoneVerificationForUserAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationGetPhoneVerificationForUserAsyncWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of UserRegistrationDto</returns>
    Task<UserRegistrationDto> RegistrationGetUserAsync(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="id"> (optional)</param>
    /// <returns>Task of ApiResponse (UserRegistrationDto)</returns>
    Task<ApiResponse<UserRegistrationDto>> RegistrationGetUserAsyncWithHttpInfo(string id = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationUpdateAddressForUserAsync(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationUpdateAddressForUserAsyncWithHttpInfo(UpdateAddressCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationUpdateContactForUserAsync(UpdateContactCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationUpdateContactForUserAsyncWithHttpInfo(UpdateContactCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationUpdateEmailVerificationForUserAsync(VerifyConfirmationCodeCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationUpdateEmailVerificationForUserAsyncWithHttpInfo(VerifyConfirmationCodeCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationUpdatePersonalDetailsForUserAsync(UpdatePersonalDetailCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationUpdatePersonalDetailsForUserAsyncWithHttpInfo(UpdatePersonalDetailCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task RegistrationUpdatePhoneVerificationForUserAsync(VerifyConfirmationCodeCommand body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> RegistrationUpdatePhoneVerificationForUserAsyncWithHttpInfo(VerifyConfirmationCodeCommand body = null);

    #endregion Asynchronous Operations
}