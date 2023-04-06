using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record MerchantHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly SimpleStringId MerchantIdId;

    #endregion

    #region Constructor

    public MerchantHasBeenCreated(SimpleStringId merchantId) : base($"The merchant with ID: [{merchantId}] has been created")
    {
        MerchantIdId = merchantId;
    }

    #endregion
}