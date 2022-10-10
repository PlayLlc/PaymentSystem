using Play.Domain.Entities;

namespace Play.Accounts.Domain.Aggregates.Users;

public record UserId : EntityId<string>
{
    #region Constructor

    public UserId(string id) : base(id)
    { }

    #endregion

    #region Instance Members

    public static UserId New()
    {
        return new UserId(GenerateStringId());
    }

    #endregion
}