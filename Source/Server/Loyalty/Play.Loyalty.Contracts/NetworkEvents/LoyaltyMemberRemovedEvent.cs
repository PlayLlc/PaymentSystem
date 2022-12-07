using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Loyalty.Contracts.Dtos;
using Play.Messaging.NServiceBus;

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