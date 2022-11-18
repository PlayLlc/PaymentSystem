using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace Play.Restful.Clients;

/// <summary>
///     Represents a set of configuration settings
/// </summary>
public class Configuration
{
    #region Static Metadata

    /// <summary>
    ///     Default creation of exceptions for a given method name and response object
    /// </summary>
    public static readonly ExceptionFactory DefaultExceptionFactory = (methodName, response) =>
    {
        int status = (int) response.StatusCode;

        if (status >= 400)
            return new ApiException(status, $"Error calling {methodName}: {response.Content} {response.Content}");

        return null;
    };

    /// <summary>
    ///     Identifier for ISO 8601 DateTime Format
    /// </summary>
    /// <remarks>See https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx#Anchor_8 for more information.</remarks>

    // ReSharper disable once InconsistentNaming
    private const string _ISO8601_DATETIME_FORMAT = "o";

    #endregion

    #region Instance Values

    /// <summary>
    ///     Gets or sets the HTTP user agent.
    /// </summary>
    /// <value>Http user agent.</value>
    public readonly string UserAgent = "Swagger-Codegen/1.0.0/csharp";

    /// <summary>
    ///     The username (HTTP basic authentication).
    /// </summary>
    /// <value>The username.</value>
    public readonly string? Username = null;

    /// <summary>
    ///     The password (HTTP basic authentication).
    /// </summary>
    /// <value>The password.</value>
    public readonly string? Password = null;

    /// <summary>
    ///     The access token for OAuth2 authentication.
    /// </summary>
    /// <value>The access token.</value>
    public readonly string? AccessToken = null;

    /// <summary>
    ///     The temporary folder path to store the files downloaded from the server.
    /// </summary>
    /// <value>Folder path.</value>
    public readonly string TempFolderPath = Path.GetTempPath();

    /// <summary>
    ///     Gets or sets the the date time format used when serializing in the ApiClient
    ///     By default, it's set to ISO 8601 - "o", for others see:
    ///     https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
    ///     and https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
    ///     No validation is done to ensure that the string you're providing is valid
    /// </summary>
    /// <value>The DateTimeFormat string</value>
    public readonly string DateTimeFormat = _ISO8601_DATETIME_FORMAT;

    /// <summary>
    ///     The base path of the API resource this client is accessing
    /// </summary>
    public readonly string BasePath;

    /// <summary>
    ///     Gets or sets the HTTP timeout (milliseconds) of ApiClient. Default to 100000 milliseconds.
    /// </summary>
    public readonly int Timeout = 100000;

    public readonly ApiClient ApiClient;

    /// <summary>
    ///     The prefix (e.g. Token) of the API key based on the authentication name.
    /// </summary>
    /// <value>The prefix of the API key.</value>
    public IDictionary<string, string> ApiKeyPrefix { get; } = new ConcurrentDictionary<string, string>();

    /// <summary>
    ///     Gets or sets the default header.
    /// </summary>
    public IDictionary<string, string> DefaultHeader { get; } = new ConcurrentDictionary<string, string>();

    /// <summary>
    ///     The API key based on the authentication name.
    /// </summary>
    /// <value>The API key.</value>
    public IDictionary<string, string> ApiKey { get; } = new ConcurrentDictionary<string, string>();

    #endregion

    #region Constructor

    //public Configuration(ApiConfiguration configuration)
    //{
    // TODO: Determine if the ApiConfiguration is using Basic or OAuth2 auth and set those values in this class
    //}

    public Configuration(string basePath)
    {
        BasePath = basePath;
        ApiClient = new ApiClient(this);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    public Configuration(string basePath, int timeout)
    {
        BasePath = basePath;
        Timeout = timeout;

        ApiClient = new ApiClient(this);
    }

    public Configuration(string basePath, string userAgent, int? timeout = null)
    {
        UserAgent = userAgent;
        BasePath = basePath;
        Timeout = timeout ?? Timeout;

        ApiClient = new ApiClient(this);
    }

    public Configuration(string basePath, string userName, string password, string? userAgent = null, string? accessToken = null, int? timeout = null)
    {
        BasePath = basePath;
        Username = userName;
        Password = password;
        UserAgent = userAgent ?? UserAgent;
        AccessToken = accessToken;
        Timeout = timeout ?? 100000;

        ApiClient = new ApiClient(this);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Configuration" /> class
    /// </summary>
    public Configuration(
        string basePath, IDictionary<string, string> apiKey, IDictionary<string, string> apiKeyPrefix, IDictionary<string, string>? defaultHeader = null)
    {
        BasePath = basePath;

        ApiKey = apiKey;
        ApiKeyPrefix = apiKeyPrefix;
        DefaultHeader = defaultHeader ?? new ConcurrentDictionary<string, string>();

        ApiClient = new ApiClient(this);
    }

    /// <exception cref="ApiException"></exception>
    public Configuration(
        string basePath, string userAgent = "Swagger-Codegen/1.0.0/csharp", string? username = null, string? password = null, string? accessToken = null,
        int? timeout = null, string? dateTimeFormat = null, string? tempFolderPath = null, IDictionary<string, string>? apiKey = null,
        IDictionary<string, string>? apiKeyPrefix = null, IDictionary<string, string>? defaultHeader = null)
    {
        BasePath = basePath;
        UserAgent = userAgent;
        Username = username;
        Password = password;
        AccessToken = accessToken;
        Timeout = timeout ?? 100000;
        DateTimeFormat = dateTimeFormat ?? _ISO8601_DATETIME_FORMAT;
        TempFolderPath = SetupTempFolderPath(tempFolderPath);
        ApiKey = apiKey ?? new ConcurrentDictionary<string, string>();
        ApiKeyPrefix = apiKeyPrefix ?? new ConcurrentDictionary<string, string>();
        DefaultHeader = defaultHeader ?? new ConcurrentDictionary<string, string>();

        ApiClient = new ApiClient(this);
    }

    #endregion

    #region Instance Members

    /// <exception cref="ApiException"></exception>
    private static string SetupTempFolderPath(string? value)
    {
        try
        {
            if (value is null)
                return Path.GetTempPath();

            if (!Directory.Exists(value))
                Directory.CreateDirectory(value);

            if (value[^1] != Path.DirectorySeparatorChar)
                value += Path.DirectorySeparatorChar;

            return value;
        }
        catch (Exception e)
        {
            throw new ApiException(500, e.Message);
        }
    }

    /// <summary>
    ///     Gets the API key with prefix.
    /// </summary>
    /// <param name="apiKeyIdentifier">API key identifier (authentication scheme).</param>
    /// <returns>API key with prefix.</returns>
    public string GetApiKeyWithPrefix(string apiKeyIdentifier)
    {
        string apiKeyValue = "";
        ApiKey.TryGetValue(apiKeyIdentifier, out apiKeyValue);
        string apiKeyPrefix = "";

        if (ApiKeyPrefix.TryGetValue(apiKeyIdentifier, out apiKeyPrefix))
            return apiKeyPrefix + " " + apiKeyValue;
        else
            return apiKeyValue;
    }

    /// <summary>
    ///     Returns a string with essential information for debugging.
    /// </summary>
    public static string ToDebugReport()
    {
        string report = "C# SDK (IO.Swagger) Debug Report:\n";
        report += "    OS: " + RuntimeInformation.OSDescription + "\n";
        report += "    Version of the API: v1\n";
        report += "    SDK Package Version: 1.0.0\n";

        return report;
    }

    #endregion
}