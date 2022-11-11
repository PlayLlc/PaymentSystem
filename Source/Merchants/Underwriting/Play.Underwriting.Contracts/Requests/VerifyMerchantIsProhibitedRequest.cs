using Play.Accounts.Contracts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Play.Underwriting.Contracts.Requests;

public record VerifyMerchantIsProhibitedRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public AddressDto? Address { get;set;}
}
