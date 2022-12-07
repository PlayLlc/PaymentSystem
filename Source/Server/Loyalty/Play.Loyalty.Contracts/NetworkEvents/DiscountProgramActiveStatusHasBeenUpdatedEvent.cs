using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts;

public class DiscountProgramActiveStatusHasBeenUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string LoyaltyProgramId { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = null!;

    public bool IsActive { get; set; }

    #endregion
}