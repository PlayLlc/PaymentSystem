using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;

namespace Play.Inventory.Contracts.Commands;

public record CreateDiscountedItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public Money DiscountedPrice { get; set; } = null!;

    #endregion
}

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
    public Money DiscountedPrice { get; set; } = null!;

    #endregion
}