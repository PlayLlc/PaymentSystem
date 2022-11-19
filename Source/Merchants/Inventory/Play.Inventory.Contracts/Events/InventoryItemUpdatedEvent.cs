using Play.Messaging.NServiceBus;

using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Contracts.Events;

public class InventoryItemUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public ItemDto Item { get; set; } = new();

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}