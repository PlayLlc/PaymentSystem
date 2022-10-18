using Play.Accounts.Domain.ValueObjects;

namespace Play.Accounts.Domain.Services;

public interface IEnsureUniqueEmails
{
    #region Instance Members

    public bool IsUnique(string email);

    #endregion
}