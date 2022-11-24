﻿using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberRemoved : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberRemoved(Member member, string merchantId, string userId) : base(
        $"The {nameof(User)} with the ID: [{userId}] has deleted the {nameof(Member)} with the ID: [{member.Id}];")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}