using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserRegistrationHasBeenRejected : BrokenBusinessRuleDomainEvent<UserRegistration, string>
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