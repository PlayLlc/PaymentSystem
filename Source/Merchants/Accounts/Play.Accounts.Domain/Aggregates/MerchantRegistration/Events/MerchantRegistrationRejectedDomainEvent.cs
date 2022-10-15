using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationRejectedDomainEvent : DomainEvent
{
    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public MerchantRegistrationRejectedDomainEvent(string id, Name companyName) : base(
        $"The {nameof(Merchant)}: [{companyName}] is prohibited from registering an account")
    {
        Id = id;
    }

    #endregion
}