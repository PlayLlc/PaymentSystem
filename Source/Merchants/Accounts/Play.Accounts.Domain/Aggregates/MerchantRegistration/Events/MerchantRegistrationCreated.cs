using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationCreated : DomainEvent
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantRegistrationCreated(MerchantRegistration merchantRegistration) : base(
        $"The {nameof(MerchantRegistration)} process has begun for the {nameof(Merchant)} with ID: [{merchantRegistration.GetId()}].")
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}