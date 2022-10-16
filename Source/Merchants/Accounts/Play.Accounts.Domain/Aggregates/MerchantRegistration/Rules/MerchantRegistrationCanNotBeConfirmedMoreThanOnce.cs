using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

internal class MerchantRegistrationCanNotBeConfirmedMoreThanOnce : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly RegistrationStatuses _ActualRegistrationStatus;

    public override string Message => "Merchant Registration cannot be confirmed more than once";

    #endregion

    #region Constructor

    public MerchantRegistrationCanNotBeConfirmedMoreThanOnce(RegistrationStatuses actualRegistrationStatus)
    {
        _ActualRegistrationStatus = actualRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return _ActualRegistrationStatus == RegistrationStatuses.Confirmed;
    }

    public override MerchantHasAlreadyBeenRegistered CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchantRegistration)
    {
        return new MerchantHasAlreadyBeenRegistered(merchantRegistration, this);
    }

    #endregion
}