using Play.Domain.Aggregates;
using Play.Identity.Domain.Aggregates.Events;

namespace Play.Identity.Domain.Aggregates.Rules;

internal class UserMustBeActive : BusinessRule<User, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user cannot perform the requested action because they have been deactivated";

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