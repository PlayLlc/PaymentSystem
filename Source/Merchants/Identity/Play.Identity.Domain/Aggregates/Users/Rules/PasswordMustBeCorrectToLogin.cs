using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Aggregates.Events;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates.Rules;

internal class PasswordMustBeCorrectToLogin : BusinessRule<User, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The login attempt failed because the user provided an invalid password";

    #endregion

    #region Constructor

    internal PasswordMustBeCorrectToLogin(IHashPasswords passwordHasher, string hashedPassword, string clearTextPassword)
    {
        _IsValid = passwordHasher.ValidateHashedPassword(hashedPassword, clearTextPassword);
    }

    #endregion

    #region Instance Members

    public override UserAttemptedLoggingInWithIncorrectPassword CreateBusinessRuleViolationDomainEvent(User user)
    {
        return new UserAttemptedLoggingInWithIncorrectPassword(user, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}