using Play.Loyalty.Contracts.Dtos;

using System.ComponentModel.DataAnnotations;

using Play.Messaging.NServiceBus;
using Play.Domain.Common.Attributes;

namespace Play.Loyalty.Contracts;

public class LoyaltyMemberRemovedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public LoyaltyMemberDto LoyaltyMember { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    #endregion
}