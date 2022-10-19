using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserRegistrationCanNotExpireMoreThanOnce : IBusinessRule
{
    #region Instance Values

    private readonly UserRegistrationStatuses _ActualUserRegistrationStatus;

    public string Message => "User Registration cannot be confirmed because it is expired";

    #endregion

    #region Constructor

    public UserRegistrationCanNotExpireMoreThanOnce(UserRegistrationStatuses actualUserRegistrationStatus)
    {
        _ActualUserRegistrationStatus = actualUserRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _ActualUserRegistrationStatus == UserRegistrationStatuses.Expired;
    }

    #endregion
}