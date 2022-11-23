using Play.Domain.Events;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberRemoved : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberRemoved(LoyaltyMember loyaltyMember, string merchantId, string userId) : base(
        $"The {nameof(User)} with the ID: [{userId}] has deleted the {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}];")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}