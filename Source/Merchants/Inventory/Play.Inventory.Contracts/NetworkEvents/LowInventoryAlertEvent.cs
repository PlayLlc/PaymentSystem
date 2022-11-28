using System.ComponentModel.DataAnnotations;

using Play.Messaging.NServiceBus;

namespace Play.Inventory.Contracts;

public class LowInventoryAlertEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    /// <summary>
    ///     The current running subtotal of the Item's quantity
    /// </summary>
    public int Quantity { get; set; }

    #endregion
}