namespace Play.AuthenticationManagement.Identity.Services;

public class RegisterResult
{
    public bool Succeeded { get; set; }

    public string[] Errors { get; set; } = Array.Empty<string>();

    internal RegisterResult(bool succeeded, IEnumerable<string> errors)
    {
        this.Succeeded = succeeded;
        this.Errors = errors.ToArray();
    }

    public static RegisterResult Success()
    {
        return new RegisterResult(true, Array.Empty<string>());
    }

    public static RegisterResult Failure(IEnumerable<string> errors)
    {
        return new RegisterResult(false, errors);
    }
}
