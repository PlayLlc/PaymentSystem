using System.Text.Json;

using Play.Domain.Events;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Domain.Aggregates;

public record MerchantBusinessInfoHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Merchant Merchant;

    #endregion

    #region Constructor

    /// <exception cref="NotSupportedException"></exception>
    public MerchantBusinessInfoHasBeenUpdated(Merchant merchant) : base(
        $"The merchant with ID: [{merchant.GetId()}] has updated its {nameof(BusinessInfo)}. [{nameof(Merchant)}: [{JsonSerializer.Serialize(merchant.AsDto())}]")
    {
        Merchant = merchant;
    }

    #endregion
}