using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantRejectedBecauseItIsProhibited : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRejectedBecauseItIsProhibited(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}