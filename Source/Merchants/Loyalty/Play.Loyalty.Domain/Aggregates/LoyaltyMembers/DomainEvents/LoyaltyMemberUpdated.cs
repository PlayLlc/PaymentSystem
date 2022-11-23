using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberUpdated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberUpdated(LoyaltyMember loyaltyMember, string merchantId, string userId) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has been updated;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}