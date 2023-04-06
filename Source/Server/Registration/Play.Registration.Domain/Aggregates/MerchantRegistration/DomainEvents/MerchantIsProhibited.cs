using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents;

public record MerchantIsProhibited : BrokenRuleOrPolicyDomainEvent<MerchantRegistration, SimpleStringId>
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantIsProhibited(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}