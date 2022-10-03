using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events;

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