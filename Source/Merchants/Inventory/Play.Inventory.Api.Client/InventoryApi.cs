/* 
 * Inventory
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.Collections.ObjectModel;

using Play.Inventory.Contracts.Dtos;
using Play.Restful.Clients;

using RestSharp.Portable;

namespace Play.Inventory.Api.Client;

/// <summary>
///     Represents a collection of functions to interact with the API endpoints
/// </summary>
public class InventoryApi : IInventoryApi
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
    ///     Initializes a new instance of the <see cref="InventoryApi" /> class.
    /// </summary>
    /// <returns></returns>
    public InventoryApi(string basePath)
    {
        Configuration = new Configuration(basePath);

        ExceptionFactory = Configuration.DefaultExceptionFactory;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="InventoryApi" /> class
    ///     using Configuration object
    /// </summary>
    /// <param name="configuration">An instance of Configuration</param>
    /// <returns></returns>
    public InventoryApi(Configuration configuration)
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
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>InventoryDto</returns>
    public InventoryDto GetInventory(string storeId)
    {
        ApiResponse<InventoryDto> localVarResponse = GetInventoryWithHttpInfo(storeId);

        return localVarResponse.Data;
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>ApiResponse of InventoryDto</returns>
    public ApiResponse<InventoryDto> GetInventoryWithHttpInfo(string storeId)
    {
        // verify the required parameter 'storeId' is set
        if (storeId == null)
            throw new ApiException(400, "Missing required parameter 'storeId' when calling InventoryApi->GetInventory");

        string localVarPath = "./Inventory/{storeId}";
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

        if (storeId != null)
            localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.DELETE, localVarQueryParams, localVarPostBody,
            localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("GetInventory", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<InventoryDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
            (InventoryDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryDto)));
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>Task of InventoryDto</returns>
    public async Task<InventoryDto> GetInventoryAsync(string storeId)
    {
        ApiResponse<InventoryDto> localVarResponse = await GetInventoryAsyncWithHttpInfo(storeId);

        return localVarResponse.Data;
    }

    /// <summary>
    /// </summary>
    /// <exception cref="ApiException">Thrown when fails to make API call</exception>
    /// <param name="storeId"></param>
    /// <returns>Task of ApiResponse (InventoryDto)</returns>
    public async Task<ApiResponse<InventoryDto>> GetInventoryAsyncWithHttpInfo(string storeId)
    {
        // verify the required parameter 'storeId' is set
        if (storeId == null)
            throw new ApiException(400, "Missing required parameter 'storeId' when calling InventoryApi->GetInventory");

        string localVarPath = "./Inventory/{storeId}";
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

        if (storeId != null)
            localVarPathParams.Add("storeId", Configuration.ApiClient.ParameterToString(storeId)); // path parameter

        // make the HTTP request
        IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.DELETE, localVarQueryParams,
            localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

        int localVarStatusCode = (int) localVarResponse.StatusCode;

        if (ExceptionFactory != null)
        {
            Exception exception = ExceptionFactory("GetInventory", localVarResponse);

            if (exception != null)
                throw exception;
        }

        return new ApiResponse<InventoryDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
            (InventoryDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(InventoryDto)));
    }

    #endregion
}