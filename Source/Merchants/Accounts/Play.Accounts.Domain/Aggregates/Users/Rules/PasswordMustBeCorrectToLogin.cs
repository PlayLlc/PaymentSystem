using Play.Accounts.Domain.Services;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class PasswordMustBeCorrectToLogin : BusinessRule<User, string>
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
        return _IsValid;
    }

    #endregion
}