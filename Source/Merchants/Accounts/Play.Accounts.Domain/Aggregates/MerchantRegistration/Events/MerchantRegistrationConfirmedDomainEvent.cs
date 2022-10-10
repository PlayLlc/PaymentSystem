using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationConfirmedDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(MerchantRegistrationConfirmedDomainEvent));

    #endregion

    #region Instance Values

    public readonly MerchantRegistrationId Id;

    #endregion

    #region Constructor

    public MerchantRegistrationConfirmedDomainEvent(MerchantRegistrationId id) : base(DomainEventTypeId)
    {
        Id = id;
    }

    #endregion
}