using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;

namespace Play.Registration.Domain.Aggregates.UserRegistration.Rules;

internal class UserRegistrationUsernameMustBeAValidEmail : BusinessRule<UserRegistration>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Username must be a valid email address";

    #endregion

    #region Constructor

    internal UserRegistrationUsernameMustBeAValidEmail(string username)
    {
        _IsValid = Email.IsValid(username);
    }

    #endregion

    #region Instance Members

    public override UserRegistrationUsernameWasInvalid CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}