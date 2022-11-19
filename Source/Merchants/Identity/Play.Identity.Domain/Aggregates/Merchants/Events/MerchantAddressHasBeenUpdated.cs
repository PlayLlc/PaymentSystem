using System.Text.Json;

using Play.Domain.Common.Entities;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record MerchantAddressHasBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Merchant Merchant;

    #endregion

    #region Constructor

    /// <exception cref="NotSupportedException"></exception>
    public MerchantAddressHasBeenUpdated(Merchant merchant) : base(
        $"The merchant with ID: [{merchant.GetId()}] has updated its {nameof(Address)}. [{nameof(Merchant)}: [{JsonSerializer.Serialize(merchant.AsDto())}]")
    {
        Merchant = merchant;
    }

    #endregion
}