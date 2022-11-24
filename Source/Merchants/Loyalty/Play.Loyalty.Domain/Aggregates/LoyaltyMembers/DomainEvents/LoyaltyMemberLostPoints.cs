using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberLostPoints : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberLostPoints(Member member, string merchantId, string userId, uint transactionId, uint points) : base(
        $"The {nameof(Member)} with the ID: [{member.Id}] has lost [{points}] points;")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}