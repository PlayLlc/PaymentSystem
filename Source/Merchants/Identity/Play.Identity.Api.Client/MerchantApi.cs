/* 
 * Identity Authentication Management
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System.Collections.ObjectModel;

using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Restful.Clients;

using RestSharp.Portable;

namespace Play.Identity.Api.Client
{
    /// <summary>
    ///     Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public partial class MerchantApi : IMerchantApi
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
        ///     Initializes a new instance of the <see cref="MerchantApi" /> class.
        /// </summary>
        /// <returns></returns>
        public MerchantApi(string basePath)
        {
            Configuration = new Configuration(basePath);

            ExceptionFactory = Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MerchantApi" /> class
        ///     using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public MerchantApi(Configuration configuration)
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
        public void MerchantAddressPut(UpdateAddressCommand body = null)
        {
            MerchantAddressPutWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MerchantAddressPutWithHttpInfo(UpdateAddressCommand body = null)
        {
            var localVarPath = "./Merchant/Address";
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
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.PUT, localVarQueryParams, localVarPostBody,
                localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantAddressPut", localVarResponse);

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
        public async Task MerchantAddressPutAsync(UpdateAddressCommand body = null)
        {
            await MerchantAddressPutAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> MerchantAddressPutAsyncWithHttpInfo(UpdateAddressCommand body = null)
        {
            var localVarPath = "./Merchant/Address";
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
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.PUT, localVarQueryParams,
                localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantAddressPut", localVarResponse);

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
        public void MerchantBusinessInfoPut(UpdateMerchantBusinessInfo body = null)
        {
            MerchantBusinessInfoPutWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MerchantBusinessInfoPutWithHttpInfo(UpdateMerchantBusinessInfo body = null)
        {
            var localVarPath = "./Merchant/BusinessInfo";
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
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.PUT, localVarQueryParams, localVarPostBody,
                localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantBusinessInfoPut", localVarResponse);

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
        public async Task MerchantBusinessInfoPutAsync(UpdateMerchantBusinessInfo body = null)
        {
            await MerchantBusinessInfoPutAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> MerchantBusinessInfoPutAsyncWithHttpInfo(UpdateMerchantBusinessInfo body = null)
        {
            var localVarPath = "./Merchant/BusinessInfo";
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
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.PUT, localVarQueryParams,
                localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantBusinessInfoPut", localVarResponse);

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
        public void MerchantCompanyNamePut(UpdateMerchantCompanyName body = null)
        {
            MerchantCompanyNamePutWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> MerchantCompanyNamePutWithHttpInfo(UpdateMerchantCompanyName body = null)
        {
            var localVarPath = "./Merchant/CompanyName";
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
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.PUT, localVarQueryParams, localVarPostBody,
                localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantCompanyNamePut", localVarResponse);

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
        public async Task MerchantCompanyNamePutAsync(UpdateMerchantCompanyName body = null)
        {
            await MerchantCompanyNamePutAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> MerchantCompanyNamePutAsyncWithHttpInfo(UpdateMerchantCompanyName body = null)
        {
            var localVarPath = "./Merchant/CompanyName";
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
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.PUT, localVarQueryParams,
                localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantCompanyNamePut", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>MerchantDto</returns>
        public MerchantDto MerchantGet(string id = null)
        {
            ApiResponse<MerchantDto> localVarResponse = MerchantGetWithHttpInfo(id);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>ApiResponse of MerchantDto</returns>
        public ApiResponse<MerchantDto> MerchantGetWithHttpInfo(string id = null)
        {
            var localVarPath = "./Merchant";
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

            if (id != null)
                localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "id", id)); // query parameter

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) Configuration.ApiClient.CallApi(localVarPath, Method.GET, localVarQueryParams, localVarPostBody,
                localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<MerchantDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (MerchantDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(MerchantDto)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>Task of MerchantDto</returns>
        public async Task<MerchantDto> MerchantGetAsync(string id = null)
        {
            ApiResponse<MerchantDto> localVarResponse = await MerchantGetAsyncWithHttpInfo(id);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>Task of ApiResponse (MerchantDto)</returns>
        public async Task<ApiResponse<MerchantDto>> MerchantGetAsyncWithHttpInfo(string id = null)
        {
            var localVarPath = "./Merchant";
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

            if (id != null)
                localVarQueryParams.AddRange(Configuration.ApiClient.ParameterToKeyValuePairs("", "id", id)); // query parameter

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await Configuration.ApiClient.CallApiAsync(localVarPath, Method.GET, localVarQueryParams,
                localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams, localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("MerchantGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<MerchantDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (MerchantDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(MerchantDto)));
        }

        #endregion
    }
}