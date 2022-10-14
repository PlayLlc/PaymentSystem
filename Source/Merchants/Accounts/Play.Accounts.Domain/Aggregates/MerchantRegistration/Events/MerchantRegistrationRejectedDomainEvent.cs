using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationRejectedDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(MerchantRegistrationRejectedDomainEvent));

    #endregion

    #region Instance Values

    public readonly string Id;

    #endregion

    #region Constructor

    public MerchantRegistrationRejectedDomainEvent(string id) : base(DomainEventTypeId)
    {
        Id = id;
    }

    #endregion
}