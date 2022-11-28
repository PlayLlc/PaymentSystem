using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts;

public class DiscountHasBeenRemovedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string DiscountId { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = null!;

    #endregion
}