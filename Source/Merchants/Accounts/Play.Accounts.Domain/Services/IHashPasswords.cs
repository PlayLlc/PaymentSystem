using Play.Accounts.Domain.Aggregates;

namespace Play.Accounts.Domain.Services;

public interface IHashPasswords
{
    #region Instance Members

    public string GeneratePasswordHash(string password);
    public bool ValidateHashedPassword(string hashedPassword, string clearTextPassword);

    #endregion
}