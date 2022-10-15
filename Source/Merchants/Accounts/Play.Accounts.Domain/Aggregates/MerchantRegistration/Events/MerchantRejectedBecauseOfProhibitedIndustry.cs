using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantRejectedBecauseOfProhibitedIndustry : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRejectedBecauseOfProhibitedIndustry(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}