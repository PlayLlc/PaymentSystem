using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class UserCannotBeCreatedWithoutApproval : BusinessRule<UserRegistration, SimpleStringId>
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

    public override UserRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) =>
        new UserRegistrationHasNotBeenApproved(merchant, this);

    public override bool IsBroken() => _Status != UserRegistrationStatuses.Approved;

    #endregion
}