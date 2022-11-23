using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberEarnedPoints : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberEarnedPoints(LoyaltyMember loyaltyMember, string merchantId, string userId, uint points) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has earned [{points}] points;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}

public record LoyaltyMemberClaimedRewards : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberClaimedRewards(LoyaltyMember loyaltyMember, string merchantId, string userId, Money claimedRewards) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has claimed [{claimedRewards}] in rewards;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}