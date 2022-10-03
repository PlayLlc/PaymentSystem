using Play.Domain;
using Play.Merchants.Onboarding.Domain.Enums;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Rules;

internal class MerchantRegistrationCanNotBeConfirmedMoreThanOnce : IBusinessRule
{
    #region Instance Values

    private readonly RegistrationStatuses _ActualRegistrationStatus;

    public string Message => "User Registration cannot be confirmed more than once";

    #endregion

    #region Constructor

    public MerchantRegistrationCanNotBeConfirmedMoreThanOnce(RegistrationStatuses actualRegistrationStatus)
    {
        _ActualRegistrationStatus = actualRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _ActualRegistrationStatus == RegistrationStatuses.Confirmed;
    }

    #endregion
}