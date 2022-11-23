using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Contracts.Commands;

public record UpdateLoyaltyMember
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    #endregion
}

public record UpdateRewardPoints
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public MoneyDto TransactionAmount { get; set; } = null!;

    #endregion
}