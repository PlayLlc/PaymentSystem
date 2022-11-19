using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record UserRegistrationHasBeenRejected : BrokenRuleOrPolicyDomainEvent<UserRegistration, SimpleStringId>
{
    #region Instance Values

    public readonly UserRegistration UserRegistration;

    #endregion

    #region Constructor

    public UserRegistrationHasBeenRejected(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    {
        UserRegistration = userRegistration;
    }

    #endregion
}