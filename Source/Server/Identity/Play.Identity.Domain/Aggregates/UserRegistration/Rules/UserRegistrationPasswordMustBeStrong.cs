using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

/// <summary>
///     PCI-DSS Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters, and be unique
///     when updated
/// </summary>
internal class UserRegistrationPasswordMustBeStrong : BusinessRule<UserRegistration>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters";

    #endregion

    #region Constructor

    internal UserRegistrationPasswordMustBeStrong(string password)
    {
        _IsValid = ClearTextPassword.IsValid(password);
    }

    #endregion

    #region Instance Members

    public override UserRegistrationPasswordWasTooWeak CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => _IsValid;

    #endregion
}