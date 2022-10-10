using Play.Domain.Entities;

namespace Play.Accounts.Domain.Aggregates.Merchants;

public record MerchantId : EntityId<string>
{
    #region Constructor

    public MerchantId(string id) : base(id)
    { }

    #endregion

    #region Instance Members

    public static MerchantId New()
    {
        return new MerchantId(GenerateStringId());
    }

    #endregion
}