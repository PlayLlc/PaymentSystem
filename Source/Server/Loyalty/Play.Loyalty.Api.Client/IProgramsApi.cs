using Play.Loyalty.Contracts.Commands;
using Play.Loyalty.Contracts.Dtos;
using Play.Restful.Clients;

namespace Play.Loyalty.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IProgramsApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ProgramsActivateRewardsProgramRewardsProgram(string programId, ActivateProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ProgramsActivateRewardsProgramRewardsProgramWithHttpInfo(string programId, ActivateProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ProgramsCreateDiscounts(string programsId, CreateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ProgramsCreateDiscountsWithHttpInfo(string programsId, CreateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ProgramsCreatePrograms(CreateLoyaltyProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ProgramsCreateProgramsWithHttpInfo(CreateLoyaltyProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="programsId"> (optional)</param>
    /// <returns>LoyaltyProgramDto</returns>
    LoyaltyProgramDto ProgramsGetPrograms(string programId, string programsId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="programsId"> (optional)</param>
    /// <returns>ApiResponse of LoyaltyProgramDto</returns>
    ApiResponse<LoyaltyProgramDto> ProgramsGetProgramsWithHttpInfo(string programId, string programsId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ProgramsRemoveDiscounts(string programsId, RemoveDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ProgramsRemoveDiscountsWithHttpInfo(string programsId, RemoveDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ProgramsUpdateDiscounts(string programsId, UpdateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ProgramsUpdateDiscountsWithHttpInfo(string programsId, UpdateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void ProgramsUpdateRewardsProgramRewardsProgram(string programId, UpdateRewardsProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> ProgramsUpdateRewardsProgramRewardsProgramWithHttpInfo(string programId, UpdateRewardsProgram body = null);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ProgramsActivateRewardsProgramRewardsProgramAsync(string programId, ActivateProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ProgramsActivateRewardsProgramRewardsProgramAsyncWithHttpInfo(string programId, ActivateProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ProgramsCreateDiscountsAsync(string programsId, CreateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ProgramsCreateDiscountsAsyncWithHttpInfo(string programsId, CreateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ProgramsCreateProgramsAsync(CreateLoyaltyProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ProgramsCreateProgramsAsyncWithHttpInfo(CreateLoyaltyProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="programsId"> (optional)</param>
    /// <returns>Task of LoyaltyProgramDto</returns>
    Task<LoyaltyProgramDto> ProgramsGetProgramsAsync(string programId, string programsId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="programsId"> (optional)</param>
    /// <returns>Task of ApiResponse (LoyaltyProgramDto)</returns>
    Task<ApiResponse<LoyaltyProgramDto>> ProgramsGetProgramsAsyncWithHttpInfo(string programId, string programsId = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ProgramsRemoveDiscountsAsync(string programsId, RemoveDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ProgramsRemoveDiscountsAsyncWithHttpInfo(string programsId, RemoveDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ProgramsUpdateDiscountsAsync(string programsId, UpdateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programsId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ProgramsUpdateDiscountsAsyncWithHttpInfo(string programsId, UpdateDiscountedItem body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task ProgramsUpdateRewardsProgramRewardsProgramAsync(string programId, UpdateRewardsProgram body = null);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="programId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> ProgramsUpdateRewardsProgramRewardsProgramAsyncWithHttpInfo(string programId, UpdateRewardsProgram body = null);

    #endregion Asynchronous Operations
}