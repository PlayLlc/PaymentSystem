using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

/// <summary>
///     PCI-DSS Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters, and be unique
///     when updated
/// </summary>
internal class PasswordMustBeStrong : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters";

    #endregion

    #region Constructor

    internal PasswordMustBeStrong(string password)
    {
        _IsValid = Password.IsValid(password);
    }

    #endregion

    #region Instance Members

    public override PasswordWasTooWeak CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new PasswordWasTooWeak(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}