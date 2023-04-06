using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents;

public record MerchantRegistrationApproved : DomainEvent
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantRegistrationApproved(MerchantRegistration merchantRegistration) : base(
        $"The {nameof(MerchantRegistration)} with ID: [{merchantRegistration.GetId()}] has been approved")
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}