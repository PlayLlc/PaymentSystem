namespace Play.Restful.Clients;

/// <summary>
///     API Response
/// </summary>
public class ApiResponse<_>
{
    #region Instance Values

    /// <summary>
    ///     Gets or sets the status code (HTTP status code)
    /// </summary>
    /// <value>The status code.</value>
    public int StatusCode { get; }

    /// <summary>
    ///     Gets or sets the HTTP headers
    /// </summary>
    /// <value>HTTP headers</value>
    public IDictionary<string, string> Headers { get; }

    /// <summary>
    ///     Gets or sets the data (parsed HTTP body)
    /// </summary>
    /// <value>The data.</value>
    public _ Data { get; }

    #endregion

    #region Constructor

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiResponse&lt;T&gt;" /> class.
    /// </summary>
    /// <param name="statusCode">HTTP status code.</param>
    /// <param name="headers">HTTP headers.</param>
    /// <param name="data">Data (parsed HTTP body)</param>
    public ApiResponse(int statusCode, IDictionary<string, string> headers, _ data)
    {
        StatusCode = statusCode;
        Headers = headers;
        Data = data;
    }

    #endregion
}