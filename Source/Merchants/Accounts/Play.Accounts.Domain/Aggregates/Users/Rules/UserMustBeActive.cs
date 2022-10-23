using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserMustBeActive : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Passwords must be at least 7 characters, contain numeric, alphabetic, and special characters";

    #endregion

    #region Constructor

    internal UserMustBeActive(bool isActive)
    {
        _IsValid = isActive;
    }

    #endregion

    #region Instance Members

    public override UserHasBeenDeactivated CreateBusinessRuleViolationDomainEvent(User merchant)
    {
        return new UserHasBeenDeactivated(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}