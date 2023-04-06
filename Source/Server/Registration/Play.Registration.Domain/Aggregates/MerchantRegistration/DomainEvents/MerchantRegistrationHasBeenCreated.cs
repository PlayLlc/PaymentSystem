using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents;

public record MerchantRegistrationHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantRegistrationHasBeenCreated(MerchantRegistration merchantRegistration) : base(
        $"The {nameof(MerchantRegistration)} process has begun for the {nameof(MerchantRegistration)} with ID: [{merchantRegistration.GetId()}].")
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}