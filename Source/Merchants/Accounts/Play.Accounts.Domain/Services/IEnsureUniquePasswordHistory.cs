namespace Play.Accounts.Domain.Services;

public interface IEnsureUniquePasswordHistory
{
    #region Instance Members

    public Task<bool> AreLastFourPasswordsUnique(string userId, string hashedPassword);

    #endregion
}