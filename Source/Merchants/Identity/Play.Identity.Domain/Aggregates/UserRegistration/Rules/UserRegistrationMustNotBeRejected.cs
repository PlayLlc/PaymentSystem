using Play.Domain.Aggregates;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class UserRegistrationMustNotBeRejected : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "User cannot complete registration because the user is prohibited";

    #endregion

    #region Constructor

    public UserRegistrationMustNotBeRejected(UserRegistrationStatus status)
    {
        if (status == UserRegistrationStatuses.Rejected)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override UserRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new UserRegistrationHasBeenRejected(merchant, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}