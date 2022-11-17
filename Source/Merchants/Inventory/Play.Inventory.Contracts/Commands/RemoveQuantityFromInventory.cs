using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Enums;

namespace Play.Inventory.Contracts.Commands;

/// <summary>
///     Remove quantities from inventory for the specified item
/// </summary>
public record RemoveQuantityFromInventory
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string StoreId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public StockActions Action { get; set; } = StockActions.Empty;

    [Required]
    public ushort Quantity { get; set; }

    #endregion
}