using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;
using Play.Globalization.Time;

namespace Play.Identity.Contracts.Dtos;

public class MerchantRegistrationDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [DateTimeUtc]
    public DateTime RegisteredDate { get; set; }

    public AddressDto? AddressDto { get; set; } = new();
    public BusinessInfoDto? BusinessInfo { get; set; }

    public string? RegistrationStatus { get; set; } = string.Empty;

    #endregion
}