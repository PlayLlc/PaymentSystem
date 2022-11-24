using Play.Loyalty.Contracts.Dtos;

using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts;

public class LoyaltyMemberUpdatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public LoyaltyMemberDto LoyaltyMember { get; set; } = null!;

    #endregion
}