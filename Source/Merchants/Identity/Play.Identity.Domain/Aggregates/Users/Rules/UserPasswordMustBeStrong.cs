using Play.Domain.Aggregates;
using Play.Identity.Domain.Aggregates.Events;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates.Rules;

/// <summary>
///     PCI-DSS Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters, and be unique
///     when updated
/// </summary>
internal class UserPasswordMustBeStrong : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters";

    #endregion

    #region Constructor

    internal UserPasswordMustBeStrong(string password)
    {
        _IsValid = ClearTextPassword.IsValid(password);
    }

    #endregion

    #region Instance Members

    public override UserPasswordWasTooWeak CreateBusinessRuleViolationDomainEvent(User merchant)
    {
        return new UserPasswordWasTooWeak(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}