using Play.Domain.Aggregates;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Enums;
using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.Rules;

internal class MerchantRegistrationCannotBeCreatedWithoutApproval : BusinessRule<MerchantRegistration>
{
    #region Instance Values

    private readonly MerchantRegistrationStatus _Status;

    public override string Message => $"A Merchant account cannot be created until the {nameof(MerchantRegistration)} has been approved";

    #endregion

    #region Constructor

    internal MerchantRegistrationCannotBeCreatedWithoutApproval(MerchantRegistrationStatus status)
    {
        _Status = status;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => _Status != MerchantRegistrationStatuses.Approved;

    #endregion
}