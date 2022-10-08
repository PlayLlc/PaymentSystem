namespace Play.Accounts.Api.Services;

public class RegisterResult
{
    #region Instance Values

    public bool Succeeded { get; set; }

    public string[] Errors { get; set; } = Array.Empty<string>();

    #endregion

    #region Constructor

    internal RegisterResult(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    #endregion

    #region Instance Members

    public static RegisterResult Success()
    {
        return new RegisterResult(true, Array.Empty<string>());
    }

    public static RegisterResult Failure(IEnumerable<string> errors)
    {
        return new RegisterResult(false, errors);
    }

    #endregion
}