using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberLostRewards : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberLostRewards(LoyaltyMember loyaltyMember, string merchantId, string userId, Money rewards) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has lost [{rewards.AsLocalFormat()}] in rewards;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}