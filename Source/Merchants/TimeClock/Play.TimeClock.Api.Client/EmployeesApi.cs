/* 
 * Time Clock Management
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: v1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using RestSharp.Portable; 
using Play.Restful.Clients;
using Play.TimeClock.Contracts.Commands;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
        public interface IEmployeesApi : IApiAccessor
    {
        #region Synchronous Operations
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns></returns>
        void EmployeesEditTimeEntries (string employeeId, EditTimeEntry body);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        ApiResponse<object> EmployeesEditTimeEntriesWithHttpInfo (string employeeId, EditTimeEntry body);
        #endregion Synchronous Operations
        #region Asynchronous Operations
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of void</returns>
        System.Threading.Tasks.Task EmployeesEditTimeEntriesAsync (string employeeId, EditTimeEntry body);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        System.Threading.Tasks.Task<ApiResponse<Object>> EmployeesEditTimeEntriesAsyncWithHttpInfo (string employeeId, EditTimeEntry body);
        #endregion Asynchronous Operations
    }

    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
        public partial class EmployeesApi : IEmployeesApi
    {
        private  ExceptionFactory _exceptionFactory = (name, response) => null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public EmployeesApi(string basePath)
        {
            this.Configuration = new Configuration(basePath);

            ExceptionFactory =  Configuration.DefaultExceptionFactory;
        }
         

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesApi"/> class
        /// using Configuration object
        /// </summary>
        /// <param name="configuration">An instance of Configuration</param>
        /// <returns></returns>
        public EmployeesApi( Configuration configuration)
        { 
                this.Configuration = configuration;

            ExceptionFactory =  Configuration.DefaultExceptionFactory;
        }

        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        public string GetBasePath()
        {
            return this.Configuration.ApiClient.RestClient.BaseUrl.ToString();
        }

        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <value>The base path</value>
        [Obsolete("SetBasePath is deprecated, please do 'Configuration.ApiClient = new ApiClient(\"http://new-path\")' instead.")]
        public void SetBasePath(string basePath)
        {
            // do nothing
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        /// <value>An instance of the Configuration</value>
        public  Configuration Configuration {get; set;}

        /// <summary>
        /// Provides a factory method hook for the creation of exceptions.
        /// </summary>
        public  ExceptionFactory ExceptionFactory
        {
            get
            {
                if (_exceptionFactory != null && _exceptionFactory.GetInvocationList().Length > 1)
                {
                    throw new InvalidOperationException("Multicast delegate for ExceptionFactory is unsupported.");
                }
                return _exceptionFactory;
            }
            set { _exceptionFactory = value; }
        }

        /// <summary>
        /// Gets the default header.
        /// </summary>
        /// <returns>Dictionary of HTTP header</returns>
        [Obsolete("DefaultHeader is deprecated, please use Configuration.DefaultHeader instead.")]
        public IDictionary<string, string> DefaultHeader()
        {
            return new ReadOnlyDictionary<string, string>(this.Configuration.DefaultHeader);
        } 

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns></returns>
        public void EmployeesEditTimeEntries (string employeeId, EditTimeEntry body)
        {
             EmployeesEditTimeEntriesWithHttpInfo(employeeId, body);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns>ApiResponse of Object(void)</returns>
        public ApiResponse<object> EmployeesEditTimeEntriesWithHttpInfo (string employeeId, EditTimeEntry body)
        {
            // verify the required parameter 'employeeId' is set
            if (employeeId == null)
                throw new ApiException(400, "Missing required parameter 'employeeId' when calling EmployeesApi->EmployeesEditTimeEntries");

            var localVarPath = "./TimeClock/Employees/{employeeId}/TimeEntries/Edit";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/_*+json"
            };
            string localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (employeeId != null) localVarPathParams.Add("employeeId", this.Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
            if (body != null && body.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(body); // http body (model) parameter
            }
            else
            {
                localVarPostBody = body; // byte array
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) this.Configuration.ApiClient.CallApi(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("EmployeesEditTimeEntries", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                null);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of void</returns>
        public async System.Threading.Tasks.Task EmployeesEditTimeEntriesAsync (string employeeId, EditTimeEntry body)
        {
             await EmployeesEditTimeEntriesAsyncWithHttpInfo(employeeId, body);

        }

        /// <summary>
        ///  
        /// </summary>
        /// <exception cref=" ApiException">Thrown when fails to make API call</exception>
        /// <param name="employeeId"></param>
        /// <param name="body"> (optional)</param>
        /// <returns>Task of ApiResponse</returns>
        public async System.Threading.Tasks.Task<ApiResponse<Object>> EmployeesEditTimeEntriesAsyncWithHttpInfo (string employeeId, EditTimeEntry body)
        {
            // verify the required parameter 'employeeId' is set
            if (employeeId == null)
                throw new ApiException(400, "Missing required parameter 'employeeId' when calling EmployeesApi->EmployeesEditTimeEntries");

            var localVarPath = "./TimeClock/Employees/{employeeId}/TimeEntries/Edit";
            var localVarPathParams = new Dictionary<string, string>();
            var localVarQueryParams = new List<KeyValuePair<string, string>>();
            var localVarHeaderParams = new Dictionary<string, string>(this.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<string, string>();
            var localVarFileParams = new Dictionary<string, FileParameter>();
            object localVarPostBody = null;

            // to determine the Content-Type header
            string[] localVarHttpContentTypes = new string[] {
                "application/json", 
                "text/json", 
                "application/_*+json"
            };
            string localVarHttpContentType = this.Configuration.ApiClient.SelectHeaderContentType(localVarHttpContentTypes);

            // to determine the Accept header
            string[] localVarHttpHeaderAccepts = new string[] {
            };
            string localVarHttpHeaderAccept = this.Configuration.ApiClient.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);

            if (employeeId != null) localVarPathParams.Add("employeeId", this.Configuration.ApiClient.ParameterToString(employeeId)); // path parameter
            if (body != null && body.GetType() != typeof(byte[]))
            {
                localVarPostBody = this.Configuration.ApiClient.Serialize(body); // http body (model) parameter
            }
            else
            {
                localVarPostBody = body; // byte array
            }

            // make the HTTP request
            IRestResponse localVarResponse = (IRestResponse) await this.Configuration.ApiClient.CallApiAsync(localVarPath,
                Method.PUT, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);

            int localVarStatusCode = (int) localVarResponse.StatusCode;

            if (ExceptionFactory != null)
            {
                Exception exception = ExceptionFactory("EmployeesEditTimeEntries", localVarResponse);
                if (exception != null) throw exception;
            }

            return new ApiResponse<object>(localVarStatusCode,
                localVarResponse.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value)),
                null);
        }

    }
}
