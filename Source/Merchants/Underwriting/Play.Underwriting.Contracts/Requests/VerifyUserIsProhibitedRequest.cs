using Play.Accounts.Contracts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Play.Underwriting.Contracts.Requests;

public record VerifyUserIsProhibitedRequest
{
    [Required]
    public PersonalDetailDto? PersonalDetails { get; set; }

    [Required]
    public AddressDto? Address { get; set; }

    [Required]
    public ContactDto? Contact { get; set; }
}
