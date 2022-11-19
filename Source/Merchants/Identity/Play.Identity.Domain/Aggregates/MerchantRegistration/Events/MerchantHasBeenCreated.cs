using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record MerchantHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Merchant Merchant;

    #endregion

    #region Constructor

    public MerchantHasBeenCreated(Merchant merchant) : base($"The merchant with ID: [{merchant.GetId()}] has been created")
    {
        Merchant = merchant;
    }

    #endregion
}