using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

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
    public MoneyDto Price { get; set; } = null!;

    #endregion
}