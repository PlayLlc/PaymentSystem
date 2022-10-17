namespace Play.Accounts.Domain.Entities;

public record UserRole
{
    #region Instance Values

    public string Id { get; }

    public string Name { get; }

    #endregion

    #region Constructor

    public UserRole(string id, string name)
    {
        Id = id;
        Name = name;
    }

    #endregion
}