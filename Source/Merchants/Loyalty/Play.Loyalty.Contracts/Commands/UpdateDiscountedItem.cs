using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Contracts.Commands;

public record UpdateDiscountedItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string DiscountId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public MoneyDto DiscountedPrice { get; set; } = null!;

    #endregion
}