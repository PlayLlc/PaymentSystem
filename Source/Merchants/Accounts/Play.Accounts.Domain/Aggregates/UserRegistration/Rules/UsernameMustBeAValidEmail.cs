using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.UserRegistration;

internal class UsernameMustBeAValidEmail : IBusinessRule
{
    #region Instance Values

    private readonly bool _IsValid;

    public string Message => "Username must be a valid email address";

    #endregion

    #region Constructor

    internal UsernameMustBeAValidEmail(string username)
    {
        _IsValid = Zipcode.IsValid(username);
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}