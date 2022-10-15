using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationConfirmedDomainEvent : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public MerchantRegistrationConfirmedDomainEvent(string id, Name companyName) : base(
        $"The {nameof(Merchant)}: [{companyName}] has been successfully registered")
    {
        Id = id;
    }

    #endregion
}