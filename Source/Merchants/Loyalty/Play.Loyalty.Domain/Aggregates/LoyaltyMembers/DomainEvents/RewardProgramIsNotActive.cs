using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Loyalty.Domain.Aggregates;

public record RewardProgramIsNotActive : BrokenRuleOrPolicyDomainEvent<Member, SimpleStringId>
{
    #region Instance Values

    public readonly Member Member;

    #endregion

    #region Constructor

    public RewardProgramIsNotActive(Member member, IBusinessRule rule) : base(member, rule)
    {
        Member = member;
    }

    #endregion
}