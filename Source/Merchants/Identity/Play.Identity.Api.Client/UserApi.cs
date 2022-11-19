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
    public partial class UserApi : IUserApi
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
        ///     Initializes a new instance of the <see cref="UserApi" /> class.
        /// </summary>
        /// <returns></returns>
        public UserApi(string basePath)
        {
            Configuration = new Configuration(basePath);

            ExceptionFactory = Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserApi" /> class
        ///     using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public UserApi(Configuration configuration)
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
        public void UserAddressPut(UpdateAddressCommand body = null)
        {
            UserAddressPutWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> UserAddressPutWithHttpInfo(UpdateAddressCommand body = null)
        {
            var localVarPath = "./User/Address";
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
                Exception exception = ExceptionFactory("UserAddressPut", localVarResponse);

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
        public async Task UserAddressPutAsync(UpdateAddressCommand body = null)
        {
            await UserAddressPutAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> UserAddressPutAsyncWithHttpInfo(UpdateAddressCommand body = null)
        {
            var localVarPath = "./User/Address";
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
                Exception exception = ExceptionFactory("UserAddressPut", localVarResponse);

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
        public void UserContactInfoPut(UpdateContactCommand body = null)
        {
            UserContactInfoPutWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> UserContactInfoPutWithHttpInfo(UpdateContactCommand body = null)
        {
            var localVarPath = "./User/ContactInfo";
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
                Exception exception = ExceptionFactory("UserContactInfoPut", localVarResponse);

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
        public async Task UserContactInfoPutAsync(UpdateContactCommand body = null)
        {
            await UserContactInfoPutAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> UserContactInfoPutAsyncWithHttpInfo(UpdateContactCommand body = null)
        {
            var localVarPath = "./User/ContactInfo";
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
                Exception exception = ExceptionFactory("UserContactInfoPut", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>UserDto</returns>
        public UserDto UserGet(string id = null)
        {
            ApiResponse<UserDto> localVarResponse = UserGetWithHttpInfo(id);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>ApiResponse of UserDto</returns>
        public ApiResponse<UserDto> UserGetWithHttpInfo(string id = null)
        {
            var localVarPath = "./User";
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
                Exception exception = ExceptionFactory("UserGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<UserDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (UserDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(UserDto)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>Task of UserDto</returns>
        public async Task<UserDto> UserGetAsync(string id = null)
        {
            ApiResponse<UserDto> localVarResponse = await UserGetAsyncWithHttpInfo(id);

            return localVarResponse.Data;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="id"> (optional)</param>
        /// <returns>Task of ApiResponse (UserDto)</returns>
        public async Task<ApiResponse<UserDto>> UserGetAsyncWithHttpInfo(string id = null)
        {
            var localVarPath = "./User";
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
                Exception exception = ExceptionFactory("UserGet", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<UserDto>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                (UserDto) Configuration.ApiClient.Deserialize(localVarResponse, typeof(UserDto)));
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns></returns>
        public void UserPersonalDetailsPut(UpdatePersonalDetailCommand body = null)
        {
            UserPersonalDetailsPutWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> UserPersonalDetailsPutWithHttpInfo(UpdatePersonalDetailCommand body = null)
        {
            var localVarPath = "./User/PersonalDetails";
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
                Exception exception = ExceptionFactory("UserPersonalDetailsPut", localVarResponse);

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
        public async Task UserPersonalDetailsPutAsync(UpdatePersonalDetailCommand body = null)
        {
            await UserPersonalDetailsPutAsyncWithHttpInfo(body);
        }

        /// <summary>
        /// </summary>
        /// <exception cref="ApiException">Thrown when fails to make API call</exception>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async Task<ApiResponse<object>> UserPersonalDetailsPutAsyncWithHttpInfo(UpdatePersonalDetailCommand body = null)
        {
            var localVarPath = "./User/PersonalDetails";
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
                Exception exception = ExceptionFactory("UserPersonalDetailsPut", localVarResponse);

                if (exception != null)
                    throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode, localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)), null);
        }

        #endregion
    }
}