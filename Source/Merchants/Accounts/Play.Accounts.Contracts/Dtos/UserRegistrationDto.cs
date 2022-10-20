using Play.Domain;
using Play.Globalization.Time;

namespace Play.Accounts.Contracts.Dtos;

public class UserRegistrationDto : IDto
{
    #region Instance Values

    public string? Id { get; set; }
    public AddressDto? Address { get; set; }
    public ContactDto? ContactInfo { get; set; }
    public PersonalDetailDto? PersonalInfo { get; set; }
    public DateTimeUtc RegisteredDate { get; set; }
    public string RegistrationStatus { get; set; } = string.Empty;

    #endregion
}