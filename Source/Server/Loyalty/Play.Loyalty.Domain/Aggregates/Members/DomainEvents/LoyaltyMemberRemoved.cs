using Play.Domain.Events;

namePlay.Loyalty.Domain.Entitiesgregates;

public record LoyaltyMemberRemoved : DomainEvent
{
    #region Instance Values

    public readonly Member Member;
    public readonly string UserId;

    #endregion

    #region Constructor

    public LoyaltyMemberRemoved(Member member, string userId) : base(
        $"The {nameof(User)} with t
        serId}] has deleted the {nameof(Member)} with the ID: [{member.Id}];")
    {
        Member = member;
        UserId = userId;
    }

    #endregion
}