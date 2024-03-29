﻿using System.ComponentModel.DataAnnotations;

using Play.Loyalty.Contracts.Dtos;
using Play.Messaging.NServiceBus;

namespace Play.Loyalty.Contracts;

public class LoyaltyMemberLostPointsEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public LoyaltyMemberDto LoyaltyMember { get; set; } = null!;

    public uint TransactionId { get; set; }

    #endregion
}