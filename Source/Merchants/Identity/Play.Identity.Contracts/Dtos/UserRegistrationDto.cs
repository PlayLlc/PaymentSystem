using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;
using Play.Globalization.Time;

namespace Play.Identity.Contracts.Dtos;

public class UserRegistrationDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = null!;

    [Required]
    [DateTimeUtc]
    public DateTime RegisteredDate { get; set; }

    [Required]
    [MinLength(1)]
    public string RegistrationStatus { get; set; } = string.Empty;

    [Required]
    public bool HasEmailBeenVerified { get; set; }

    [Required]
    public bool HasPhoneBeenVerified { get; set; }

    public AddressDto? Address { get; set; }
    public ContactDto? ContactInfo { get; set; }
    public PersonalDetailDto? PersonalInfo { get; set; }

    #endregion
}