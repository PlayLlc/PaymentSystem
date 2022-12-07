using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

namespace Play.Loyalty.Contracts.Commands;

public record CreateDiscountedItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public MoneyDto DiscountedPrice { get; set; } = null!;

    #endregion
}