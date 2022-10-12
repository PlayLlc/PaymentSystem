namespace Play.Identity.Api.Identity.Services;

public class Result
{
    #region Instance Values

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; } = Array.Empty<string>();

    #endregion

    #region Constructor

    internal Result(IEnumerable<string> errors)
    {
        Succeeded = false;
        Errors = errors.ToArray();
    }

    internal Result()
    { }

    #endregion
}