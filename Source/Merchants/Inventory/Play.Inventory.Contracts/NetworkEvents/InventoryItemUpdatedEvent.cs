using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Inventory.Contracts;

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