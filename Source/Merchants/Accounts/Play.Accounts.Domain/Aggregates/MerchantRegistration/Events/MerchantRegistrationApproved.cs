using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationApproved : DomainEvent
{
    #region Instance Values

    public readonly string MerchantRegistrationId;

    #endregion

    #region Constructor

    public MerchantRegistrationApproved(string merchantRegistrationId, Name companyName) : base(
        $"The {nameof(Merchant)}: [{companyName}] has been successfully registered")
    {
        MerchantRegistrationId = merchantRegistrationId;
    }

    #endregion
}