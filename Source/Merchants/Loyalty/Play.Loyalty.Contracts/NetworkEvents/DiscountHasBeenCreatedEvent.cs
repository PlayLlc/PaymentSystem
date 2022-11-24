using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Loyalty.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts.NetworkEvents;

public class DiscountHasBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public DiscountDto Discount { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = null!;

    #endregion
}