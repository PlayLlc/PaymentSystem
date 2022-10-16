namespace Play.Identity.Domain;

public interface IEnsureUniqueEmails
{
    #region Instance Members

    public bool IsUnique(Email email);

    #endregion
}