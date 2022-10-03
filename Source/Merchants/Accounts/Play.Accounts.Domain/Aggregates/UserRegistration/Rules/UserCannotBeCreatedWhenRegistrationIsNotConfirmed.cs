using Play.Domain;
using Play.Merchants.Onboarding.Domain.Enums;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

internal class UserCannotBeCreatedWhenRegistrationIsNotConfirmed : IBusinessRule
{
    #region Instance Values

    private readonly RegistrationStatuses _RegistrationStatus;

    public string Message => "User cannot be created when registration is not confirmed";

    #endregion

    #region Constructor

    internal UserCannotBeCreatedWhenRegistrationIsNotConfirmed(RegistrationStatuses registrationStatus)
    {
        _RegistrationStatus = registrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _RegistrationStatus != RegistrationStatuses.Confirmed;
    }

    #endregion
}