using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;
using Play.Globalization.Time;

namespace Play.Identity.Contracts.Dtos;

public class UserRegistrationDto : IDto
{
    #region Instance Values

    public string? Id { get; set; }
    public AddressDto? Address { get; set; }
    public ContactDto? ContactInfo { get; set; }
    public PersonalDetailDto? PersonalInfo { get; set; }

    [Required]
    [DateTimeUtc]
    public DateTime RegisteredDate { get; set; }

    public string RegistrationStatus { get; set; } = string.Empty;
    public bool HasEmailBeenVerified { get; set; }
    public bool HasPhoneBeenVerified { get; set; }

    #endregion
}