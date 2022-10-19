namespace Play.Accounts.Domain.Services;

public interface IHashPasswords
{
    #region Instance Members

    public string GeneratePasswordHash(string password);

    #endregion
}