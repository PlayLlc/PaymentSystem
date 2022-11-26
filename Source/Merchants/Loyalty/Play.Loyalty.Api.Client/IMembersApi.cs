using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Restful.Clients;

namespace IO.Swagger.Api;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IMembersApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void MembersAddRewards(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> MembersAddRewardsWithHttpInfo(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void MembersClaimRewards(string loyaltyMemberId, ClaimRewards body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> MembersClaimRewardsWithHttpInfo(string loyaltyMemberId, ClaimRewards body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void MembersCreateMembers(CreateLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> MembersCreateMembersWithHttpInfo(CreateLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <returns>LoyaltyMemberDto</returns>
    LoyaltyMemberDto MembersGetMembers(string loyaltyMemberId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <returns>ApiResponse of LoyaltyMemberDto</returns>
    ApiResponse<LoyaltyMemberDto> MembersGetMembersWithHttpInfo(string loyaltyMemberId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void MembersRemoveMembers(string loyaltyMemberId, RemoveLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> MembersRemoveMembersWithHttpInfo(string loyaltyMemberId, RemoveLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void MembersRemoveRewards(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> MembersRemoveRewardsWithHttpInfo(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void MembersUpdateMembers(string loyaltyMemberId, UpdateLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> MembersUpdateMembersWithHttpInfo(string loyaltyMemberId, UpdateLoyaltyMember body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task MembersAddRewardsAsync(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> MembersAddRewardsAsyncWithHttpInfo(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task MembersClaimRewardsAsync(string loyaltyMemberId, ClaimRewards body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> MembersClaimRewardsAsyncWithHttpInfo(string loyaltyMemberId, ClaimRewards body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task MembersCreateMembersAsync(CreateLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> MembersCreateMembersAsyncWithHttpInfo(CreateLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <returns>Task of LoyaltyMemberDto</returns>
    Task<LoyaltyMemberDto> MembersGetMembersAsync(string loyaltyMemberId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <returns>Task of ApiResponse (LoyaltyMemberDto)</returns>
    Task<ApiResponse<LoyaltyMemberDto>> MembersGetMembersAsyncWithHttpInfo(string loyaltyMemberId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task MembersRemoveMembersAsync(string loyaltyMemberId, RemoveLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> MembersRemoveMembersAsyncWithHttpInfo(string loyaltyMemberId, RemoveLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task MembersRemoveRewardsAsync(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> MembersRemoveRewardsAsyncWithHttpInfo(string loyaltyMemberId, UpdateRewardsPoints body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task MembersUpdateMembersAsync(string loyaltyMemberId, UpdateLoyaltyMember body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="loyaltyMemberId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> MembersUpdateMembersAsyncWithHttpInfo(string loyaltyMemberId, UpdateLoyaltyMember body = null);

    #endregion Asynchronous Operations
}