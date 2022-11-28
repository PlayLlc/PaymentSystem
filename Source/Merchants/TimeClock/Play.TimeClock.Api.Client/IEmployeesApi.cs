using Play.Restful.Clients;
using Play.TimeClock.Contracts.Commands;

namespace Play.TimeClock.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IEmployeesApi : IApiAccessor
{
    #region Synchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void EmployeesEditTimeEntries(string employeeId, EditTimeEntry body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> EmployeesEditTimeEntriesWithHttpInfo(string employeeId, EditTimeEntry body);

    #endregion Synchronous Operations

    #region Asynchronous Operations

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task EmployeesEditTimeEntriesAsync(string employeeId, EditTimeEntry body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> EmployeesEditTimeEntriesAsyncWithHttpInfo(string employeeId, EditTimeEntry body);

    #endregion Asynchronous Operations
}