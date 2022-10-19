using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserRegistrationCanNotBeConfirmedMoreThanOnce : IBusinessRule
{
    #region Instance Values

    private readonly UserRegistrationStatuses _ActualUserRegistrationStatus;

    public string Message => "User Registration cannot be confirmed more than once";

    #endregion

    #region Constructor

    public UserRegistrationCanNotBeConfirmedMoreThanOnce(UserRegistrationStatuses actualUserRegistrationStatus)
    {
        _ActualUserRegistrationStatus = actualUserRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _ActualUserRegistrationStatus == UserRegistrationStatuses.Approved;
    }

    #endregion
}