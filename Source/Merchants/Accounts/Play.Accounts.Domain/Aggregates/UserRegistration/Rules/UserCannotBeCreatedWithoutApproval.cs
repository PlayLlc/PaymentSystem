using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserCannotBeCreatedWithoutApproval : BusinessRule<UserRegistration, string>
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

    public override UserRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new UserRegistrationHasNotBeenApproved(merchant, this);
    }

    public override bool IsBroken()
    {
        return _Status != UserRegistrationStatuses.Approved;
    }

    #endregion
}