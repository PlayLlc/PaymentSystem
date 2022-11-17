using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class UserRegistrationUsernameMustBeAValidEmail : BusinessRule<UserRegistration, string>
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

    public override UserRegistrationUsernameWasInvalid CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new UserRegistrationUsernameWasInvalid(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}