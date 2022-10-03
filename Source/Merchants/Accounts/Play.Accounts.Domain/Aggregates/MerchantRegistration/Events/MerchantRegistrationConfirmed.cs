using Play.Domain.Events;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantRegistrationConfirmed : DomainEvent
{
    #region Static Metadata

    public static readonly DomainEventTypeId DomainEventTypeId = CreateEventTypeId(typeof(MerchantRegistrationConfirmed));

    #endregion

    #region Instance Values

    public readonly MerchantRegistrationId Id;

    #endregion

    #region Constructor

    public MerchantRegistrationConfirmed(MerchantRegistrationId id) : base(DomainEventTypeId)
    {
        Id = id;
    }

    #endregion
}