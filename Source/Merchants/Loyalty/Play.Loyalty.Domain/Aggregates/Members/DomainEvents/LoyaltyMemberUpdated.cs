using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberUpdated : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberUpdated(Member member, string merchantId, string userId) : base($"The {nameof(Member)} with the ID: [{member.Id}] has been updated;")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}