using System.Text.Json;

using Play.Accounts.Domain.Entities;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Events;

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