using Play.Accounts.Contracts.Dtos;

namespace Play.Underwriting.Contracts.Commands;

public record VerifyUserIsProhibitedCommand
{
    public PersonalDetailDto? PersonalDetails { get; set; }

    public AddressDto? Address { get; set; } 

    public ContactDto? Contact { get; set; }
}
