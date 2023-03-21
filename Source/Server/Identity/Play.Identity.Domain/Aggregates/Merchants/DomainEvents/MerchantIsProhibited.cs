using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates.Merchants.DomainEvents;

public record MerchantIsProhibited : BrokenRuleOrPolicyDomainEvent<Merchant, SimpleStringId>
{
    #region Instance Values

    public readonly Merchant MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantIsProhibited(Merchant merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}