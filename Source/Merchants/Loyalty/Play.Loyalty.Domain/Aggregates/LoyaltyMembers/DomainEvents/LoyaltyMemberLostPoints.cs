using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberLostPoints : DomainEvent
{
    #region Instance Values

    public readonly LoyaltyMember LoyaltyMember;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberLostPoints(LoyaltyMember loyaltyMember, string merchantId, string userId, uint transactionId, uint points) : base(
        $"The {nameof(LoyaltyMember)} with the ID: [{loyaltyMember.Id}] has lost [{points}] points;")
    {
        LoyaltyMember = loyaltyMember;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}