using Play.Domain.Aggregates;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Enums;
using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Domain.Aggregates.UserRegistration.Rules;

internal class UserRegistrationMustNotBeRejected : BusinessRule<UserRegistration>
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

    public override UserRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}