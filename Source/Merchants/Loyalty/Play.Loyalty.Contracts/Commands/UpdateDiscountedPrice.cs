using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Contracts.Commands;

public record UpdateDiscountedPrice
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string DiscountId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public MoneyDto DiscountedPrice { get; set; } = null!;

    #endregion
}