using Play.Domain.Events;

namePlay.Loyalty.Domain.Entitiesgregates;

public record LoyaltyMemberCreated : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberCreated(Member member, string merchantId, string userId) : base(
        $"The {nameof(Member)} with
        {member.Id}] has been created for the {nameof(Merchant)} with the ID: [{merchantId} by the {nameof(User)} with the ID: {userId};")
    {
        Member = member;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}