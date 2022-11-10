using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantRegistrationHasBeenCreated(MerchantRegistration merchantRegistration) : base(
        $"The {nameof(MerchantRegistration)} process has begun for the {nameof(Merchant)} with ID: [{merchantRegistration.GetId()}].")
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}