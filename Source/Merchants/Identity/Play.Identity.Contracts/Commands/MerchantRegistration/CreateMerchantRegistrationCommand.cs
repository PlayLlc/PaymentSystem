using System.ComponentModel.DataAnnotations;

using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Contracts.Commands.MerchantRegistration;

public record CreateMerchantRegistrationCommand
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string CompanyName { get; set; } = string.Empty;

    #endregion
}