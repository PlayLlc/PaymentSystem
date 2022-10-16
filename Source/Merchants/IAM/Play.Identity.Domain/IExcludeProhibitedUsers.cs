namespace Play.Identity.Domain;

public interface IExcludeProhibitedUsers
{
    #region Instance Members

    public bool IsUserProhibited(Address address, ContactInfo contactInfo);

    #endregion
}