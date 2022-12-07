using Play.Domain.Events;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberLostRewards : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberLostRewards(Member member, string merchantId, string userId, uint transactionId, Money rewards) : base(
        $"The {nameof(Member)} with the ID: [{member.Id}] has lost [{rewards.AsLocalFormat()}] in rewards;")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}