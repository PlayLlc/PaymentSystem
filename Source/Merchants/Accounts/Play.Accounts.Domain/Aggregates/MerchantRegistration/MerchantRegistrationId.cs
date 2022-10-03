using Play.Domain.Entities;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationId : EntityId<string>
{
    #region Constructor

    public MerchantRegistrationId(string id) : base(id)
    { }

    #endregion

    #region Instance Members

    public static MerchantRegistrationId New()
    {
        return new MerchantRegistrationId(GenerateStringId());
    }

    #endregion
}