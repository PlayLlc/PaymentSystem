﻿using Play.Domain.Events;

using System.ComponentModel.DataAnnotations;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberEarnedPoints : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberEarnedPoints(LoyaltyMember loyaltyMember, string merchantId, string userId, uint transactionId, uint points) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has earned [{points}] points;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}