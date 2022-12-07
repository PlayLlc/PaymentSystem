namespace Play.Restful.Clients;

/// <summary>
///     Represents configuration aspects required to interact with the API endpoints.
/// </summary>
public interface IApiAccessor
{
    #region Instance Values

    /// <summary>
    ///     Gets or sets the configuration object
    /// </summary>
    /// <value>An instance of the Configuration</value>
    Configuration Configuration { get; set; }

    /// <summary>
    ///     Provides a factory method hook for the creation of exceptions.
    /// </summary>
    ExceptionFactory ExceptionFactory { get; set; }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Gets the base path of the API client.
    /// </summary>
    /// <value>The base path</value>
    string GetBasePath();

    #endregion
}