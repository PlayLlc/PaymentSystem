using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class MerchantRegistrationMustNotBeRejected : BusinessRule<MerchantRegistration>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Merchant cannot be created because the registration was rejected";

    #endregion

    #region Constructor

    public MerchantRegistrationMustNotBeRejected(MerchantRegistrationStatus status)
    {
        if (status == MerchantRegistrationStatuses.Rejected)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}