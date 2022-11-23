using System.ComponentModel.DataAnnotations;

using Play.Globalization.Time;
using Play.Inventory.Contracts.Enums;
using Play.Messaging.NServiceBus;

namespace Play.Inventory.Contracts.Events;

public class StockItemUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string InventoryId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string StockId { get; set; } = string.Empty;

    [Required]
    public StockActions Action { get; set; } = StockActions.Empty;

    /// <summary>
    ///     The quantity that was updated for the Item
    /// </summary>
    public ushort QuantityUpdated { get; set; }

    /// <summary>
    ///     The current running subtotal of the Item's quantity
    /// </summary>
    public int TotalQuantity { get; set; }

    public DateTimeUtc UpdatedAt { get; set; }

    #endregion
}