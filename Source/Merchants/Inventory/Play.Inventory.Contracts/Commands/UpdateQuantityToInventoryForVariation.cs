using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Enums;

namespace Play.Inventory.Contracts.Commands;

public record UpdateQuantityToInventoryForVariation
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public StockActions Action { get; set; } = StockActions.Empty;

    [Required]
    public ushort Quantity { get; set; }

    #endregion
}

public record UpdateStockItemQuantity
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public StockActions Action { get; set; } = StockActions.Empty;

    [Required]
    public ushort Quantity { get; set; }

    #endregion
}