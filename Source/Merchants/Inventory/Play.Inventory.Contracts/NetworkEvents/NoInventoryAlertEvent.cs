using System.ComponentModel.DataAnnotations;

using Play.Messaging.NServiceBus;

namespace Play.Inventory.Contracts;

public class NoInventoryAlertEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    #endregion
}