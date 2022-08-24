namespace MerchantPortal.Security.Contracts.Authentication;

public sealed class RegistrationResult
{
    public string UserId { get; init; }

    public IEnumerable<string> ErrorMessages { get; init; }
}
