using Play.Restful.Clients;
using Play.TimeClock.Contracts.Commands;
using Play.TimeClock.Contracts.Dtos;

namespace Play.TimeClock.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public interface IHomeApi : IApiAccessor
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
    void EmployeesClockInEmployees(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> EmployeesClockInEmployeesWithHttpInfo(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void EmployeesClockOutEmployees(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> EmployeesClockOutEmployeesWithHttpInfo(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void EmployeesCreateEmployees(CreateEmployee body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> EmployeesCreateEmployeesWithHttpInfo(CreateEmployee body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>EmployeeDto</returns>
    EmployeeDto EmployeesGetEmployees(string employeeId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>ApiResponse of EmployeeDto</returns>
    ApiResponse<EmployeeDto> EmployeesGetEmployeesWithHttpInfo(string employeeId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    void EmployeesRemoveEmployees(string employeeId, RemoveEmployee body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    ApiResponse<object> EmployeesRemoveEmployeesWithHttpInfo(string employeeId, RemoveEmployee body);

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
    Task EmployeesClockInEmployeesAsync(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> EmployeesClockInEmployeesAsyncWithHttpInfo(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task EmployeesClockOutEmployeesAsync(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> EmployeesClockOutEmployeesAsyncWithHttpInfo(string employeeId, UpdateTimeClock body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task EmployeesCreateEmployeesAsync(CreateEmployee body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> EmployeesCreateEmployeesAsyncWithHttpInfo(CreateEmployee body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>Task of EmployeeDto</returns>
    Task<EmployeeDto> EmployeesGetEmployeesAsync(string employeeId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>Task of ApiResponse (EmployeeDto)</returns>
    Task<ApiResponse<EmployeeDto>> EmployeesGetEmployeesAsyncWithHttpInfo(string employeeId);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    Task EmployeesRemoveEmployeesAsync(string employeeId, RemoveEmployee body);

    /// <summary>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    Task<ApiResponse<object>> EmployeesRemoveEmployeesAsyncWithHttpInfo(string employeeId, RemoveEmployee body);

    #endregion Asynchronous Operations
}