using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Contracts.Commands;

public record UpdateRewardsPoints
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public uint TransactionId { get; set; }

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public MoneyDto TransactionAmount { get; set; } = null!;

    #endregion
}