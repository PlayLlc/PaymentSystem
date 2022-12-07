using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Inventory.Contracts;

public class InventoryItemVariationCreatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    public VariationDto Variation { get; set; } = new();

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}