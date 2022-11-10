namespace Play.Accounts.Api.Services;

public class Result
{
    #region Instance Values

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; } = Array.Empty<string>();

    #endregion

    #region Constructor

    internal Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    #endregion

    #region Instance Members

    public static Result Success()
    {
        return new Result(true, Array.Empty<string>());
    }

    public static Result Failure(IEnumerable<string> errors)
    {
        return new Result(false, errors);
    }

    #endregion
}