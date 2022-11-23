using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberEarnedRewards : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberEarnedRewards(LoyaltyMember loyaltyMember, string merchantId, string userId, Money rewards) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has earned [{rewards.AsLocalFormat()}] in rewards;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}