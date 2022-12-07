using System.ComponentModel.DataAnnotations;

using Play.Messaging.NServiceBus;

namespace Play.Inventory.Contracts;

public class InventoryItemVariationRemovedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    public string VariationId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}