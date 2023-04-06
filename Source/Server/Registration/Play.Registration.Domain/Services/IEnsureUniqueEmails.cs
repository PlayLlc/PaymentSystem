namespace Play.Registration.Domain.Services;

public interface IEnsureUniqueEmails
{
    #region Instance Members

    public Task<bool> IsUnique(string email);

    #endregion
}