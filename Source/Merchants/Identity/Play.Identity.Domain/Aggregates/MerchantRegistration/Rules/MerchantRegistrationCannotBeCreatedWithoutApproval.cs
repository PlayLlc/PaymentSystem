using Play.Domain.Aggregates;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class MerchantRegistrationCannotBeCreatedWithoutApproval : BusinessRule<MerchantRegistration, string>
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

    public override MerchantRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant)
    {
        return new MerchantRegistrationHasNotBeenApproved(merchant, this);
    }

    public override bool IsBroken()
    {
        return _Status != MerchantRegistrationStatuses.Approved;
    }

    #endregion
}