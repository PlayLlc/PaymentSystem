namespace MerchantPortal.Security.Contracts;

public class SecurityToken
{
    public string? Token { get; init; }

    public DateTime ExpiryDate { get; init; }
}
