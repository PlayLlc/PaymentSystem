using Play.Domain;
using Play.Merchants.Onboarding.Domain.Enums;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Rules;

internal class MerchantCannotBeCreatedWhenRegistrationIsNotConfirmed : IBusinessRule
{
    #region Instance Values

    private readonly RegistrationStatus _RegistrationStatus;

    public string Message => "User cannot be created when registration is not confirmed";

    #endregion

    #region Constructor

    internal MerchantCannotBeCreatedWhenRegistrationIsNotConfirmed(RegistrationStatuses registrationStatus)
    {
        _RegistrationStatus = registrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _RegistrationStatus != RegistrationStatus.Confirmed;
    }

    #endregion
}