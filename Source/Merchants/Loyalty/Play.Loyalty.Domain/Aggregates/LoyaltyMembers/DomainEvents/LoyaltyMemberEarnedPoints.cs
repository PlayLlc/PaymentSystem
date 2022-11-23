using Play.Domain.Events;
using Play.Globalization.Currency;

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

public record LoyaltyMemberClaimedRewards : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberClaimedRewards(LoyaltyMember loyaltyMember, string merchantId, string userId, uint transactionId, Money claimedRewards) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has claimed [{claimedRewards}] in rewards;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}