using Play.Accounts.Domain.Enums;
using Play.Domain;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

internal class UserRegistrationCanNotExpireMoreThanOnce : IBusinessRule
{
    #region Instance Values

    private readonly RegistrationStatuses _ActualRegistrationStatus;

    public string Message => "User Registration cannot be confirmed because it is expired";

    #endregion

    #region Constructor

    public UserRegistrationCanNotExpireMoreThanOnce(RegistrationStatuses actualRegistrationStatus)
    {
        _ActualRegistrationStatus = actualRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _ActualRegistrationStatus == RegistrationStatuses.Expired;
    }

    #endregion
}