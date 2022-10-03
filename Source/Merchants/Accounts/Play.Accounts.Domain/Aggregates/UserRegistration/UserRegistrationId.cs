using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public record UserRegistrationId : EntityId<string>
{
    #region Constructor

    public UserRegistrationId(string id) : base(id)
    { }

    #endregion

    #region Instance Members

    public static UserRegistrationId New()
    {
        return new UserRegistrationId(GenerateStringId());
    }

    #endregion
}