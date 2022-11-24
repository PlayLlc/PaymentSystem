using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberClaimedRewards : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberClaimedRewards(Member member, string merchantId, string userId, uint transactionId, Money claimedRewards) : base(
        $"The {nameof(Member)} with the ID: [{member.Id}] has claimed [{claimedRewards}] in rewards;")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}