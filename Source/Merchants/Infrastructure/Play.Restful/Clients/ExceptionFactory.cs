using RestSharp.Portable;

namespace Play.Restful.Clients;

/// <summary>
///     A delegate to ExceptionFactory method
/// </summary>
/// <param name="methodName">Method name</param>
/// <param name="response">Response</param>
/// <returns>Exceptions</returns>
public delegate Exception ExceptionFactory(string methodName, IRestResponse response);