using Play.Domain.Events;

using System.ComponentModel.DataAnnotations;

namespace Play.Loyalty.Domain.Aggregates;

public record LoyaltyMemberEarnedPoints : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;
    public readonly uint TransactionId;

    #endregion

    #region Constructor

    public LoyaltyMemberEarnedPoints(Member member, string merchantId, string userId, uint transactionId, uint points) : base(
        $"The {nameof(Member)} with the ID: [{member.Id}] has earned [{points}] points;")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
        TransactionId = transactionId;
    }

    #endregion
}