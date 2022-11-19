using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace Play.Restful.Clients;

/// <summary>
///     API client is mainly responsible for making the HTTP call to the API backend.
/// </summary>
public partial class ApiClient
{
    #region Instance Values

    private readonly JsonSerializerSettings _SerializerSettings = new() {ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor};

    /// <summary>
    ///     Gets or sets an instance of the IReadableConfiguration.
    /// </summary>
    /// <value>An instance of the IReadableConfiguration.</value>
    /// <remarks>
    ///     <see cref="Configuration" /> helps us to avoid modifying possibly global
    ///     configuration values from within a given client. It does not guarantee thread-safety
    ///     of the <see cref="Configuration" /> instance in any way.
    /// </remarks>
    public Configuration Configuration { get; set; }

    /// <summary>
    ///     Gets or sets the RestClient.
    /// </summary>
    /// <value>An instance of the RestClient</value>
    public RestClient RestClient { get; set; }

    #endregion

    #region Constructor

    // This private constructor is for testing only
    private ApiClient()
    {
        Configuration = new Configuration("/");
        RestClient = new RestClient("/");
        RestClient.IgnoreResponseStatusCode = true;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiClient" /> class
    ///     with default base path (/).
    /// </summary>
    /// <param name="config">An instance of Configuration.</param>
    public ApiClient(Configuration config)
    {
        Configuration = config;
        RestClient = new RestClient(Configuration.BasePath);
        RestClient.IgnoreResponseStatusCode = true;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiClient" /> class
    ///     with default configuration.
    /// </summary>
    /// <param name="basePath">The base path.</param>
    /// <exception cref="ApiException"></exception>
    public ApiClient(string basePath)
    {
        try
        {
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentException("basePath cannot be empty");

            RestClient = new RestClient(basePath);
            RestClient.IgnoreResponseStatusCode = true;
            Configuration = new Configuration(basePath);
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Allows for extending request processing for <see cref="ApiClient" /> generated code.
    /// </summary>
    /// <param name="request">The RestSharp request object</param>
    partial void InterceptRequest(IRestRequest request);

    /// <summary>
    ///     Allows for extending response processing for <see cref="ApiClient" /> generated code.
    /// </summary>
    /// <param name="request">The RestSharp request object</param>
    /// <param name="response">The RestSharp response object</param>
    partial void InterceptResponse(IRestRequest request, IRestResponse response);

    // Creates and sets up a RestRequest prior to a call.
    /// <exception cref="ApiException"></exception>
    private RestRequest PrepareRequest(
        string path, Method method, List<KeyValuePair<string, string>> queryParams, object postBody, Dictionary<string, string> headerParams,
        Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
    {
        try
        {
            RestRequest request = new RestRequest(path, method);

            // disable ResetSharp.Portable built-in serialization
            request.Serializer = null;

            // add path parameter, if any
            foreach (KeyValuePair<string, string> param in pathParams)
                request.AddParameter(param.Key, param.Value, ParameterType.UrlSegment);

            // add header parameter, if any
            foreach (KeyValuePair<string, string> param in headerParams)
                request.AddHeader(param.Key, param.Value);

            // add query parameter, if any
            foreach (KeyValuePair<string, string> param in queryParams)
                request.AddQueryParameter(param.Key, param.Value);

            // add form parameter, if any
            foreach (KeyValuePair<string, string> param in formParams)
                request.AddParameter(param.Key, param.Value);

            // add file parameter, if any
            foreach (KeyValuePair<string, FileParameter> param in fileParams)
                request.AddFile(param.Value);

            if (postBody != null) // http body (model or byte[]) parameter
                request.AddParameter(new Parameter
                {
                    Value = postBody,
                    Type = ParameterType.RequestBody,
                    ContentType = contentType
                });

            return request;
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Makes the HTTP request (Sync).
    /// </summary>
    /// <param name="path">URL path.</param>
    /// <param name="method">HTTP method.</param>
    /// <param name="queryParams">Query parameters.</param>
    /// <param name="postBody">HTTP body (POST request).</param>
    /// <param name="headerParams">Header parameters.</param>
    /// <param name="formParams">Form parameters.</param>
    /// <param name="fileParams">File parameters.</param>
    /// <param name="pathParams">Path parameters.</param>
    /// <param name="contentType">Content Type of the request</param>
    /// <returns>Object</returns>
    public object CallApi(
        string path, Method method, List<KeyValuePair<string, string>> queryParams, object postBody, Dictionary<string, string> headerParams,
        Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
    {
        try
        {
            var request = PrepareRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, pathParams, contentType);

            // set timeout
            RestClient.Timeout = TimeSpan.FromMilliseconds(Configuration.Timeout);

            // set user agent
            RestClient.UserAgent = Configuration.UserAgent;

            InterceptRequest(request);
            var response = RestClient.Execute(request).Result;
            InterceptResponse(request, response);

            return (object) response;
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Makes the asynchronous HTTP request.
    /// </summary>
    /// <param name="path">URL path.</param>
    /// <param name="method">HTTP method.</param>
    /// <param name="queryParams">Query parameters.</param>
    /// <param name="postBody">HTTP body (POST request).</param>
    /// <param name="headerParams">Header parameters.</param>
    /// <param name="formParams">Form parameters.</param>
    /// <param name="fileParams">File parameters.</param>
    /// <param name="pathParams">Path parameters.</param>
    /// <param name="contentType">Content type.</param>
    /// <returns>The Task instance.</returns>
    public async Task<object> CallApiAsync(
        string path, Method method, List<KeyValuePair<string, string>> queryParams, object postBody, Dictionary<string, string> headerParams,
        Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType)
    {
        var request = PrepareRequest(path, method, queryParams, postBody, headerParams, formParams, fileParams, pathParams, contentType);
        InterceptRequest(request);
        var response = await RestClient.Execute(request);
        InterceptResponse(request, response);

        return (object) response;
    }

    /// <summary>
    ///     Escape string (url-encoded).
    /// </summary>
    /// <param name="str">String to be escaped.</param>
    /// <returns>Escaped string.</returns>
    public string EscapeString(string str)
    {
        try
        {
            return UrlEncode(str);
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Create FileParameter based on Stream.
    /// </summary>
    /// <param name="name">Parameter name.</param>
    /// <param name="stream">Input stream.</param>
    /// <returns>FileParameter.</returns>
    /// <exception cref="ApiException"></exception>
    public FileParameter ParameterToFile(string name, Stream stream)
    {
        try
        {
            if (stream is FileStream fileStream)
                return FileParameter.Create(name, ReadAsBytes(fileStream), Path.GetFileName(fileStream.Name));
            else
                return FileParameter.Create(name, ReadAsBytes(stream), "no_file_name_provided");
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    public FileParameter ParameterToFile(string name, byte[] stream)
    {
        return FileParameter.Create(name, stream, "no_file_name_provided");
    }

    /// <summary>
    ///     If parameter is DateTime, output in a formatted string (default ISO 8601), customizable with
    ///     Configuration.DateTime.
    ///     If parameter is a list, join the list with ",".
    ///     Otherwise just return the string.
    /// </summary>
    /// <param name="obj">The parameter (header, path, query, form).</param>
    /// <returns>Formatted string.</returns>
    /// <exception cref="ApiException"></exception>
    public string ParameterToString(object obj)
    {
        try
        {
            if (obj is DateTime time)

                // Return a formatted date string - Can be customized with Configuration.DateTimeFormat
                // Defaults to an ISO 8601, using the known as a Round-trip date/time pattern ("o")
                // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8
                // For example: 2009-06-15T13:45:30.0000000
            {
                return time.ToString(Configuration.DateTimeFormat);
            }
            else if (obj is DateTimeOffset offset)

                // Return a formatted date string - Can be customized with Configuration.DateTimeFormat
                // Defaults to an ISO 8601, using the known as a Round-trip date/time pattern ("o")
                // https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8
                // For example: 2009-06-15T13:45:30.0000000
            {
                return offset.ToString(Configuration.DateTimeFormat);
            }
            else if (obj is IList list)
            {
                var flattenedString = new StringBuilder();

                foreach (object? param in list)
                {
                    if (flattenedString.Length > 0)
                        flattenedString.Append(",");
                    flattenedString.Append(param);
                }

                return flattenedString.ToString();
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Deserialize the JSON string into a proper object.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="type">Object type.</param>
    /// <returns>Object representation of the JSON string.</returns>
    /// <exception cref="ApiException"></exception>
    public object Deserialize(IRestResponse response, Type type)
    {
        try
        {
            IHttpHeaders headers = response.Headers;

            if (type == typeof(byte[])) // return byte array
                return response.RawBytes;

            // TODO: ? if (type.IsAssignableFrom(typeof(Stream)))
            if (type == typeof(Stream))
            {
                if (headers != null)
                {
                    string filePath = string.IsNullOrEmpty(Configuration.TempFolderPath) ? Path.GetTempPath() : Configuration.TempFolderPath;
                    var regex = new Regex(@"Content-Disposition=.*filename=['""]?([^'""\s]+)['""]?$");

                    foreach (var header in headers)
                    {
                        var match = regex.Match(header.ToString());

                        if (match.Success)
                        {
                            string fileName = filePath + SanitizeFilename(match.Groups[1].Value.Replace("\"", "").Replace("'", ""));
                            File.WriteAllBytes(fileName, response.RawBytes);

                            return new FileStream(fileName, FileMode.Open);
                        }
                    }
                }

                var stream = new MemoryStream(response.RawBytes);

                return stream;
            }

            if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
                return DateTime.Parse(response.Content, null, DateTimeStyles.RoundtripKind);

            if ((type == typeof(string)) || type.Name.StartsWith("System.Nullable")) // return primitive type
                return ConvertType(response.Content, type);

            // at this point, it must be a model (json)
            return JsonConvert.DeserializeObject(response.Content, type, _SerializerSettings)
                   ?? throw new NullReferenceException(
                       $"The {nameof(ApiClient)} returned a null pointer when attempting to {nameof(Deserialize)} the response");
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Check if the given MIME is a JSON MIME.
    ///     JSON MIME examples:
    ///     application/json
    ///     application/json; charset=UTF8
    ///     APPLICATION/JSON
    ///     application/vnd.company+json
    /// </summary>
    /// <param name="mime">MIME</param>
    /// <returns>Returns True if MIME type is json.</returns>
    /// <exception cref="ApiException"></exception>
    public bool IsJsonMime(string? mime)
    {
        var jsonRegex = new Regex("(?i)^(application/json|[^;/ \t]+/[^;/ \t]+[+]json)[ \t]*(;.*)?$");

        try
        {
            return (mime != null) && (jsonRegex.IsMatch(mime) || mime.Equals("application/json-patch+json"));
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Select the Content-Type header's value from the given content-type array:
    ///     if JSON type exists in the given array, use it;
    ///     otherwise use the first one defined in 'consumes'
    /// </summary>
    /// <param name="contentTypes">The Content-Type array to select from.</param>
    /// <returns>The Content-Type header to use.</returns>
    /// <exception cref="ApiException"></exception>
    public string SelectHeaderContentType(string[] contentTypes)
    {
        try
        {
            if (contentTypes.Length == 0)
                return "application/json";

            foreach (string contentType in contentTypes)
                if (IsJsonMime(contentType.ToLower()))
                    return contentType;

            return contentTypes[0]; // use the first content type specified in 'consumes'
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Select the Accept header's value from the given accepts array:
    ///     if JSON exists in the given array, use it;
    ///     otherwise use all of them (joining into a string)
    /// </summary>
    /// <param name="accepts">The accepts array to select from.</param>
    /// <returns>The Accept header to use.</returns>
    /// <exception cref="ApiException"></exception>
    public string SelectHeaderAccept(string[] accepts)
    {
        try
        {
            if (accepts.Length == 0)
                return null;

            if (accepts.Contains("application/json", StringComparer.OrdinalIgnoreCase))
                return "application/json";

            return string.Join(",", accepts);
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Encode string in base64 format.
    /// </summary>
    /// <param name="text">String to be encoded.</param>
    /// <returns>Encoded string.</returns>
    /// <exception cref="ApiException"></exception>
    public static string Base64Encode(string text)
    {
        try
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Dynamically cast the object into target type.
    /// </summary>
    /// <param name="fromObject">Object to be casted</param>
    /// <param name="toObject">Target type</param>
    /// <returns>Casted object</returns>
    /// <exception cref="ApiException"></exception>
    public static dynamic ConvertType(dynamic fromObject, Type toObject)
    {
        try
        {
            return Convert.ChangeType(fromObject, toObject);
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Convert stream to byte array
    /// </summary>
    /// <param name="inputStream">Input stream to be converted</param>
    /// <returns>Byte array</returns>
    /// <exception cref="ApiException"></exception>
    public static byte[] ReadAsBytes(Stream inputStream)
    {
        try
        {
            byte[] buf = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int count;
                while ((count = inputStream.Read(buf, 0, buf.Length)) > 0)
                    ms.Write(buf, 0, count);

                return ms.ToArray();
            }
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     URL encode a string
    ///     Credit/Ref: https://github.com/restsharp/RestSharp/blob/master/RestSharp/Extensions/StringExtensions.cs#L50
    /// </summary>
    /// <param name="input">String to be URL encoded</param>
    /// <returns>Byte array</returns>
    /// <exception cref="ApiException"></exception>
    public static string UrlEncode(string input)
    {
        try
        {
            const int maxLength = 32766;

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length <= maxLength)
                return Uri.EscapeDataString(input);

            StringBuilder sb = new StringBuilder(input.Length * 2);
            int index = 0;

            while (index < input.Length)
            {
                int length = Math.Min(input.Length - index, maxLength);
                string subString = input.Substring(index, length);

                sb.Append(Uri.EscapeDataString(subString));
                index += subString.Length;
            }

            return sb.ToString();
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Sanitize filename by removing the path
    /// </summary>
    /// <param name="filename">Filename</param>
    /// <returns>Filename</returns>
    /// <exception cref="ApiException"></exception>
    public static string SanitizeFilename(string filename)
    {
        try
        {
            Match match = Regex.Match(filename, @".*[/\\](.*)$");

            if (match.Success)
                return match.Groups[1].Value;
            else
                return filename;
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Convert params to key/value pairs.
    ///     Use collectionFormat to properly format lists and collections.
    /// </summary>
    /// <param name="name">Key name.</param>
    /// <param name="value">Value object.</param>
    /// <returns>A list of KeyValuePairs</returns>
    /// <exception cref="ApiException"></exception>
    public IEnumerable<KeyValuePair<string, string>> ParameterToKeyValuePairs(string collectionFormat, string name, object value)
    {
        try
        {
            var parameters = new List<KeyValuePair<string, string>>();

            if (IsCollection(value) && (collectionFormat == "multi"))
            {
                var valueCollection = value as IEnumerable;
                parameters.AddRange(valueCollection?.Cast<object>().Select(item => new KeyValuePair<string, string>(name, ParameterToString(item)))
                                    ?? Array.Empty<KeyValuePair<string, string>>());
            }
            else
            {
                parameters.Add(new KeyValuePair<string, string>(name, ParameterToString(value)));
            }

            return parameters;
        }

        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Check if generic object is a collection.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if object is a collection type</returns>
    private static bool IsCollection(object value)
    {
        return value is IList or ICollection;
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     Serialize an input (model) into JSON string
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <returns>JSON string.</returns>
    /// <exception cref="ApiException"></exception>
    public string? Serialize(object? obj)
    {
        try
        {
            return obj is not null ? JsonConvert.SerializeObject(obj) : null;
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    #endregion
}