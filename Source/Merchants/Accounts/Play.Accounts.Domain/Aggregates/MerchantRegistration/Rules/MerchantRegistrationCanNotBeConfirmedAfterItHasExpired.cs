using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

internal class MerchantRegistrationCanNotBeConfirmedAfterItHasExpired : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly RegistrationStatuses _ActualRegistrationStatus;

    public override string Message => "Merchant Registration cannot be confirmed because it is expired";

    #endregion

    #region Constructor

    public MerchantRegistrationCanNotBeConfirmedAfterItHasExpired(RegistrationStatuses actualRegistrationStatus)
    {
        _ActualRegistrationStatus = actualRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return _ActualRegistrationStatus == RegistrationStatuses.Expired;
    }

    public override MerchantRejectedBecauseTheRegistrationPeriodExpired CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchantRegistration)
    {
        return new MerchantRejectedBecauseTheRegistrationPeriodExpired(merchantRegistration, this);
    }

    #endregion
}