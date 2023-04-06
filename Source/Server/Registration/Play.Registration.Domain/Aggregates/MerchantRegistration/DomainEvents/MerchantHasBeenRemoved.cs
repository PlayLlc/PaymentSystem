using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents;

public record MerchantHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Merchant Merchant;

    #endregion

    #region Constructor

    public MerchantHasBeenRemoved(Merchant merchant) : base($"The merchant with ID: [{merchant.GetId()}] has been removed")
    {
        Merchant = merchant;
    }

    #endregion
}