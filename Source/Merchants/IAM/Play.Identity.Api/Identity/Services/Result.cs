using System.Net;

namespace Play.Identity.Api.Identity.Services;

public class RestfulResult : Result
{
    #region Instance Values

    public readonly HttpStatusCode? StatusCode;

    #endregion

    #region Constructor

    public RestfulResult() : base()
    { }

    public RestfulResult(HttpStatusCode statusCode, IEnumerable<string> errors) : base(errors)
    {
        StatusCode = statusCode;
    }

    #endregion
}

public class Result
{
    #region Instance Values

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; } = Array.Empty<string>();

    #endregion

    #region Constructor

    public Result(IEnumerable<string> errors)
    {
        Succeeded = false;
        Errors = errors.ToArray();
    }

    public Result()
    {
        Succeeded = true;
    }

    #endregion
}