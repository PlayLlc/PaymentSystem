using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UserCannotBeCreatedWhenRegistrationIsNotConfirmed : IBusinessRule
{
    #region Instance Values

    private readonly UserRegistrationStatuses _UserRegistrationStatus;

    public string Message => "User cannot be created when registration is not confirmed";

    #endregion

    #region Constructor

    internal UserCannotBeCreatedWhenRegistrationIsNotConfirmed(UserRegistrationStatuses userRegistrationStatus)
    {
        _UserRegistrationStatus = userRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _UserRegistrationStatus != UserRegistrationStatuses.Approved;
    }

    #endregion
}