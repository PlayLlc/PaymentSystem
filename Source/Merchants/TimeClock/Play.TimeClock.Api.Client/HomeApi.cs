/* 
 * Time Clock Management
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.Collections.ObjectModel;

using Play.Restful.Clients;
using Play.TimeClock.Contracts.Commands;
using Play.TimeClock.Contracts.Dtos;

using RestSharp.Portable;

namespace Play.TimeClock.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public class HomeApi : IHomeApi
{
    #region Instance Values

    private ExceptionFactory _ExceptionFactory = (name, response) => null;

    /// <summary>
    ///     Gets or sets the configuration object
    /// </summary>
    /// <value>An instance of the Configuration</value>
    public Configuration Configuration { get; set; }

    /// <summary>
    ///     Provides a factory method hook for the creation of exceptions.
    /// </summary>
    public ExceptionFactory ExceptionFactory
    {
        get
        {
            if ((_ExceptionFactory != null) && (_ExceptionFactory.GetInvocationList().Length > 1))
                throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");

            return _ExceptionFactory;
        }
        set => _ExceptionFactory = value;
    }

    #endregion

    #region Constructor

    /// <summary>
    ///     Initializes a new instance of the <see cref="HomeApi" /> class.
    /// </summary>
    /// <returns></returns>
    public HomeApi(string basePath)
    {
        Configuration = new Configuration(basePath);

        ExceptionFactory = Configuration.DefaultExceptionFactory;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="HomeApi" /> class
    ///     using Configuration object
    /// </summary>
    /// <param name="configuration">An instance of Configuration</param>
    /// <returns></returns>
    public HomeApi(Configuration configuration)
    {
        Configuration = configuration;

        ExceptionFactory = Configuration.DefaultExceptionFactory;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Gets the base path of the API client.
    /// </summary>
    /// <value>The base path</value>
    public string GetBasePath() => Configuration.ApiClient.RestClient.BaseUrl.ToString();

    /// <summary>
    ///     Sets the base path of the API client.
    /// </summary>
    /// <value>The base path</value>
    [Obsolete("SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
    public void SetBasePath(string basePath)
    {
        // do nothing
    }

    /// <summary>
    ///     Gets the default header.
    /// </summary>
    /// <returns>Dictionary of HTTP header</returns>
    [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
    public IDictionary<string, string> DefaultHeader() => new ReadOnlyDictionary<string, string>(Configuration.DefaultHeader);

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    public void EmployeesClockInEmployees(string employeeId, UpdateTimeClock body)
    {
        EmployeesClockInEmployeesWithHttpInfo(employeeId, body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    public ApiResponse<object> EmployeesClockInEmployeesWithHttpInfo(string employeeId, UpdateTimeClock body)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesClockInEmployees");

        string? localVarPath = "./TimeClock/{employeeId}/ClockIn";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.PUT, localVarQueryParams, localVarPostBody,
            localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesClockInEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    public async Task EmployeesClockInEmployeesAsync(string employeeId, UpdateTimeClock body)
    {
        await EmployeesClockInEmployeesAsyncWithHttpInfo(employeeId, body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    public async Task<ApiResponse<object>> EmployeesClockInEmployeesAsyncWithHttpInfo(string employeeId, UpdateTimeClock body)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesClockInEmployees");

        string? localVarPath = "./TimeClock/{employeeId}/ClockIn";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.PUT, localVarQueryParams,
            localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesClockInEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    public void EmployeesClockOutEmployees(string employeeId, UpdateTimeClock body)
    {
        EmployeesClockOutEmployeesWithHttpInfo(employeeId, body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    public ApiResponse<object> EmployeesClockOutEmployeesWithHttpInfo(string employeeId, UpdateTimeClock body)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesClockOutEmployees");

        string? localVarPath = "./TimeClock/{employeeId}/ClockOut";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.PUT, localVarQueryParams, localVarPostBody,
            localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesClockOutEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    public async Task EmployeesClockOutEmployeesAsync(string employeeId, UpdateTimeClock body)
    {
        await EmployeesClockOutEmployeesAsyncWithHttpInfo(employeeId, body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    public async Task<ApiResponse<object>> EmployeesClockOutEmployeesAsyncWithHttpInfo(string employeeId, UpdateTimeClock body)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesClockOutEmployees");

        string? localVarPath = "./TimeClock/{employeeId}/ClockOut";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.PUT, localVarQueryParams,
            localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesClockOutEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    public void EmployeesCreateEmployees(CreateEmployee body)
    {
        EmployeesCreateEmployeesWithHttpInfo(body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    public ApiResponse<object> EmployeesCreateEmployeesWithHttpInfo(CreateEmployee body)
    {
        string? localVarPath = "./TimeClock";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.POST, localVarQueryParams, localVarPostBody,
            localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesCreateEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    public async Task EmployeesCreateEmployeesAsync(CreateEmployee body)
    {
        await EmployeesCreateEmployeesAsyncWithHttpInfo(body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    public async Task<ApiResponse<object>> EmployeesCreateEmployeesAsyncWithHttpInfo(CreateEmployee body)
    {
        string? localVarPath = "./TimeClock";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.POST, localVarQueryParams,
            localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesCreateEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>EmployeeDto</returns>
    public EmployeeDto EmployeesGetEmployees(string employeeId)
    {
        ApiResponse<EmployeeDto> localVarResponse = EmployeesGetEmployeesWithHttpInfo(employeeId);

        return localVarResponse.Data;
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>ApiResponse of EmployeeDto</returns>
    public ApiResponse<EmployeeDto> EmployeesGetEmployeesWithHttpInfo(string employeeId)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesGetEmployees");

        string? localVarPath = "./TimeClock/{employeeId}";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = { };
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = {"text/plain", "application/json", "text/json"};
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.GET, localVarQueryParams, localVarPostBody,
            localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesGetEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<EmployeeDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
            (EmployeeDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(EmployeeDto)));
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>Task of EmployeeDto</returns>
    public async Task<EmployeeDto> EmployeesGetEmployeesAsync(string employeeId)
    {
        ApiResponse<EmployeeDto> localVarResponse = await EmployeesGetEmployeesAsyncWithHttpInfo(employeeId);

        return localVarResponse.Data;
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <returns>Task of ApiResponse (EmployeeDto)</returns>
    public async Task<ApiResponse<EmployeeDto>> EmployeesGetEmployeesAsyncWithHttpInfo(string employeeId)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesGetEmployees");

        string? localVarPath = "./TimeClock/{employeeId}";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = { };
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = {"text/plain", "application/json", "text/json"};
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.GET, localVarQueryParams,
            localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesGetEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<EmployeeDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
            (EmployeeDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(EmployeeDto)));
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns></returns>
    public void EmployeesRemoveEmployees(string employeeId, RemoveEmployee body)
    {
        EmployeesRemoveEmployeesWithHttpInfo(employeeId, body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>ApiResponse of Object(void)</returns>
    public ApiResponse<object> EmployeesRemoveEmployeesWithHttpInfo(string employeeId, RemoveEmployee body)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesRemoveEmployees");

        string? localVarPath = "./TimeClock/{employeeId}";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.DELETE, localVarQueryParams, localVarPostBody,
            localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesRemoveEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of void</returns>
    public async Task EmployeesRemoveEmployeesAsync(string employeeId, RemoveEmployee body)
    {
        await EmployeesRemoveEmployeesAsyncWithHttpInfo(employeeId, body);
    }

    /// <summary>
    /// </summary>
    /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
    /// <param name="employeeId"></param>
    /// <param name="body"> (optional)</param>
    /// <returns>Task of ApiResponse</returns>
    public async Task<ApiResponse<object>> EmployeesRemoveEmployeesAsyncWithHttpInfo(string employeeId, RemoveEmployee body)
    {
        // verify the required parameter 'employeeId' is set
        if (employeeId == null)
            throw new ApiException(400, "Missing required parameter 'employeeId' when calling HomeApi->EmployeesRemoveEmployees");

        string? localVarPath = "./TimeClock/{employeeId}";
        Dictionary<string, string> localVarPathParams = new Dictionary<string, string>();
        List<KeyValuePair<string, string>> localVarQueryParams = new List<KeyValuePair<string, string>>();
        Dictionary<string, string> localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
        Dictionary<string, string> localVarFormParams = new Dictionary<string, string>();
        Dictionary<string, FileParameter> localVarFileParams = new Dictionary<string, FileParameter>();
        object localVarPostBody = null;

        // to determine the Content-Type header
        string[] localVarHttpContentTypes = {"application/json", "text/json", "application/_*+json"};
        string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

        // to determine the Accept header
        string[] localVarHttpHeaderAccepts = { };
        string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
        if (localVarHttpHeaderAccept != null)
            localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

        if (employeeId != null)
            localVarPathParams.Add("employeeId", Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
        if ((body != null) && (body.GetType() != typeof(byte[])))
            localVarPostBody = Configuration.ApiClient.Serialize(body); // http body (model) parameter
        else
            localVarPostBody = body; // byte array

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.DELETE, localVarQueryParams,
            localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("EmployeesRemoveEmployees", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
    }

    #endregion
}