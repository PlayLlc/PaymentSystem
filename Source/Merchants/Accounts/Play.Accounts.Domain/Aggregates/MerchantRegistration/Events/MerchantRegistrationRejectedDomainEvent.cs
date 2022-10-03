using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantRegistrationRejectedDomainEvent : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(MerchantRegistrationRejectedDomainEvent));

    #endregion

    #region Instance Values

    public readonly MerchantRegistrationId Id;

    #endregion

    #region Constructor

    public MerchantRegistrationRejectedDomainEvent(MerchantRegistrationId id) : base(DomainEventTypeId)
    {
        Id = id;
    }

    #endregion
}