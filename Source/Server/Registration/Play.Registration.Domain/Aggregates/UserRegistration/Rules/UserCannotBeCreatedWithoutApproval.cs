using Play.Domain.Aggregates;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Enums;
using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Domain.Aggregates.UserRegistration.Rules;

internal class UserCannotBeCreatedWithoutApproval : BusinessRule<UserRegistration>
{
    #region Instance Values

    private readonly UserRegistrationStatus _Status;

    public override string Message => $"A User account cannot be created until the {nameof(UserRegistration)} has been approved";

    #endregion

    #region Constructor

    internal UserCannotBeCreatedWithoutApproval(UserRegistrationStatus status)
    {
        _Status = status;
    }

    #endregion

    #region Instance Members

    public override UserRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => _Status != UserRegistrationStatuses.Approved;

    #endregion
}