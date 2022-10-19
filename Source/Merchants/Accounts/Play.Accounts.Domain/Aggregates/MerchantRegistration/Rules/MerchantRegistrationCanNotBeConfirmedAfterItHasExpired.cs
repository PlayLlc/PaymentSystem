using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantRegistrationCanNotBeConfirmedAfterItHasExpired : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly MerchantRegistrationStatus _Status;

    public override string Message => "Merchant Registration cannot be confirmed because it is expired";

    #endregion

    #region Constructor

    public MerchantRegistrationCanNotBeConfirmedAfterItHasExpired(MerchantRegistrationStatus status)
    {
        _Status = status;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return _Status == UserRegistrationStatuses.Expired;
    }

    public override MerchantRejectedBecauseTheRegistrationPeriodExpired CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchantRegistration)
    {
        return new MerchantRejectedBecauseTheRegistrationPeriodExpired(merchantRegistration, this);
    }

    #endregion
}