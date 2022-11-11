using Play.Accounts.Contracts.Dtos;

namespace Play.Underwriting.Contracts.Commands;

public record VerifyMerchantIsProhibitedCommand
{
    public string Name { get; set; } = string.Empty;

    public AddressDto? Address { get;set;}
}
