using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;
using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Contracts.Commands.MerchantRegistration;

public record UpdateMerchantRegistrationCommand
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public AddressDto Address { get; set; } = new();

    [Required]
    public BusinessInfoDto BusinessInfo { get; set; } = new();

    #endregion
}