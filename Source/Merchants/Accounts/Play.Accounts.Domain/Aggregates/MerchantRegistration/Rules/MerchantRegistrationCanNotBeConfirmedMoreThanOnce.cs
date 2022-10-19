using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantRegistrationCanNotBeConfirmedMoreThanOnce : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly UserRegistrationStatuses _ActualUserRegistrationStatus;

    public override string Message => "Merchant Registration cannot be confirmed more than once";

    #endregion

    #region Constructor

    public MerchantRegistrationCanNotBeConfirmedMoreThanOnce(UserRegistrationStatuses actualUserRegistrationStatus)
    {
        _ActualUserRegistrationStatus = actualUserRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return _ActualUserRegistrationStatus == UserRegistrationStatuses.Approved;
    }

    public override MerchantHasAlreadyBeenRegistered CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchantRegistration)
    {
        return new MerchantHasAlreadyBeenRegistered(merchantRegistration, this);
    }

    #endregion
}