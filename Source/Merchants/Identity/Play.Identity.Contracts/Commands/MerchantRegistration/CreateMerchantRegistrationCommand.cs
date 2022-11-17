using System.ComponentModel.DataAnnotations;

using Play.Identity.Contracts.Dtos;

namespace Play.Identity.Contracts.Commands.MerchantRegistration;

public record CreateMerchantRegistrationCommand
{
    #region Instance Values

    [Required]
    public UserDto User { get; set; } = new();

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    #endregion
}