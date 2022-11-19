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

using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Restful.Clients;

using RestSharp.Portable;

namespace Play.Inventory.Api.Client
{
    /// <summary>
    ///     Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class CategoriesApi : ICategoriesApi
    {
        #region Instance Values

        private ExceptionFactory _exceptionFactory = (name, response) => null;

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
                if ((_exceptionFactory != null) && (_exceptionFactory.GetInvocationList().Length > 1))
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");

                return _exceptionFactory;
            }
            set => _exceptionFactory = value;
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoriesApi" /> class.
        /// </summary>
        /// <returns></returns>
        public CategoriesApi(string basePath)
        {
            Configuration = new Configuration(basePath);

            ExceptionFactory = Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CategoriesApi" /> class
        ///     using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public CategoriesApi(Configuration configuration)
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
        public string GetBasePath()
        {
            return Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

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
        public IDictionary<string, string> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(Configuration.DefaultHeader);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns></returns>
        public void CategoriesCreatePost(CreateCategory body = null)
        {
            CategoriesCreatePostWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> CategoriesCreatePostWithHttpInfo(CreateCategory body = null)
        {
            var localVarPath = "./Categories/Create";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {"application/json", "text/json", "application/_*+json"};
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] { };
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
                Exception exception = ExceptionFactory("CategoriesCreatePost", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of void</returns>
        public async Task CategoriesCreatePostAsync(CreateCategory body = null)
        {
            await CategoriesCreatePostAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> CategoriesCreatePostAsyncWithHttpInfo(CreateCategory body = null)
        {
            var localVarPath = "./Categories/Create";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {"application/json", "text/json", "application/_*+json"};
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] { };
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
                Exception exception = ExceptionFactory("CategoriesCreatePost", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns></returns>
        public void CategoriesRemoveDelete(RemoveCategory body = null)
        {
            CategoriesRemoveDeleteWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> CategoriesRemoveDeleteWithHttpInfo(RemoveCategory body = null)
        {
            var localVarPath = "./Categories/Remove";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {"application/json", "text/json", "application/_*+json"};
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] { };
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

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
                Exception exception = ExceptionFactory("CategoriesRemoveDelete", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of void</returns>
        public async Task CategoriesRemoveDeleteAsync(RemoveCategory body = null)
        {
            await CategoriesRemoveDeleteAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> CategoriesRemoveDeleteAsyncWithHttpInfo(RemoveCategory body = null)
        {
            var localVarPath = "./Categories/Remove";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {"application/json", "text/json", "application/_*+json"};
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] { };
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

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
                Exception exception = ExceptionFactory("CategoriesRemoveDelete", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId"></param>
        /// <returns>CategoryDto</returns>
        public CategoryDto InventoryCategoriesCategoryIdGet(string categoryId)
        {
            ApiResponse<CategoryDto> localVarResponse = InventoryCategoriesCategoryIdGetWithHttpInfo(categoryId);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId"></param>
        /// <returns>ApiResponse of CategoryDto</returns>
        public ApiResponse<CategoryDto> InventoryCategoriesCategoryIdGetWithHttpInfo(string categoryId)
        {
            // verify the required parameter 'categoryId' is set
            if (categoryId == null)
                throw new ApiException(400, "Missing required parameter 'categoryId' when calling CategoriesApi->InventoryCategoriesCategoryIdGet");

            var localVarPath = "./Inventory/Categories/{categoryId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] { };
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {"text/plain", "application/json", "text/json"};
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (categoryId != null)
                localVarPathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.GET, localVarQueryParams, localVarPostBody,
                localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("InventoryCategoriesCategoryIdGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<CategoryDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (CategoryDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(CategoryDto)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId"></param>
        /// <returns>Task of CategoryDto</returns>
        public async Task<CategoryDto> InventoryCategoriesCategoryIdGetAsync(string categoryId)
        {
            ApiResponse<CategoryDto> localVarResponse = await InventoryCategoriesCategoryIdGetAsyncWithHttpInfo(categoryId);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="categoryId"></param>
        /// <returns>Task of ApiResponse (CategoryDto)</returns>
        public async Task<ApiResponse<CategoryDto>> InventoryCategoriesCategoryIdGetAsyncWithHttpInfo(string categoryId)
        {
            // verify the required parameter 'categoryId' is set
            if (categoryId == null)
                throw new ApiException(400, "Missing required parameter 'categoryId' when calling CategoriesApi->InventoryCategoriesCategoryIdGet");

            var localVarPath = "./Inventory/Categories/{categoryId}";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] { };
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {"text/plain", "application/json", "text/json"};
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (categoryId != null)
                localVarPathParams.Add("categoryId", Configuration.ApiClient.ParameterToString(categoryId)); // path parameter

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.GET, localVarQueryParams,
                localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("InventoryCategoriesCategoryIdGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<CategoryDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (CategoryDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(CategoryDto)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="merchantId"> (optional)</param>
        /// <returns>List&lt;CategoryDto&gt;</returns>
        public List<CategoryDto> InventoryCategoriesGet(string merchantId = null)
        {
            ApiResponse<List<CategoryDto>> localVarResponse = InventoryCategoriesGetWithHttpInfo(merchantId);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="merchantId"> (optional)</param>
        /// <returns>ApiResponse of List&lt;CategoryDto&gt;</returns>
        public ApiResponse<List<CategoryDto>> InventoryCategoriesGetWithHttpInfo(string merchantId = null)
        {
            var localVarPath = "./Inventory/Categories";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] { };
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {"text/plain", "application/json", "text/json"};
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (merchantId != null)
                localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "merchantId", merchantId)); // query parameter

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.GET, localVarQueryParams, localVarPostBody,
                localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("InventoryCategoriesGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<List<CategoryDto>>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (List<CategoryDto>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<CategoryDto>)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="merchantId"> (optional)</param>
        /// <returns>Task of List&lt;CategoryDto&gt;</returns>
        public async Task<List<CategoryDto>> InventoryCategoriesGetAsync(string merchantId = null)
        {
            ApiResponse<List<CategoryDto>> localVarResponse = await InventoryCategoriesGetAsyncWithHttpInfo(merchantId);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="merchantId"> (optional)</param>
        /// <returns>Task of ApiResponse (List&lt;CategoryDto&gt;)</returns>
        public async Task<ApiResponse<List<CategoryDto>>> InventoryCategoriesGetAsyncWithHttpInfo(string merchantId = null)
        {
            var localVarPath = "./Inventory/Categories";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] { };
            string localVarHttpContentType = Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {"text/plain", "application/json", "text/json"};
            string localVarHttpHeaderAccept = Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (merchantId != null)
                localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "merchantId", merchantId)); // query parameter

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.GET, localVarQueryParams,
                localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("InventoryCategoriesGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<List<CategoryDto>>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (List<CategoryDto>) Configuration.ApiClient.Deserialize(localVarResponse, typeof(List<CategoryDto>)));
        }

        #endregion
    }
}