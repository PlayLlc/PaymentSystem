using System.Text.Json;

using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.Events;

public record MerchantCompanyNameBeenUpdated : DomainEvent
{
    #region Instance Values

    public readonly Merchant Merchant;

    #endregion

    #region Constructor

    /// <exception cref="NotSupportedException"></exception>
    public MerchantCompanyNameBeenUpdated(Merchant merchant) : base(
        $"The merchant with ID: [{merchant.GetId()}] has updated its Company Name: [{nameof(Merchant)}: [{JsonSerializer.Serialize(merchant.AsDto())}]")
    {
        Merchant = merchant;
    }

    #endregion
}