using Play.Accounts.Domain.ValueObjects;

namespace Play.Accounts.Domain.Services;

public interface IEnsureUniqueEmails
{
    #region Instance Members

    public Task<bool> IsUnique(string email);

    #endregion
}