using System.ComponentModel.DataAnnotations;

using Play.Accounts.Domain.Entities;

namespace Play.Accounts.Contracts.Commands.Merchant;

public record UpdateMerchantBusinessInfo
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public BusinessInfoDto BusinessInfo { get; set; } = new();

    #endregion
}