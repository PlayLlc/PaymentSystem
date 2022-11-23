using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberCreated : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberCreated(LoyaltyMember loyaltyMember, string merchantId, string userId) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has been created for the {nameof(Merchant)} with the ID: [{merchantId} by the {nameof(User)} with the ID: {userId};")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}