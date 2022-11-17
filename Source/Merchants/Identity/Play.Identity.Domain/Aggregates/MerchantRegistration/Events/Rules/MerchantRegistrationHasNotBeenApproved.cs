using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Identity.Domain.Aggregates;

public record MerchantRegistrationHasNotBeenApproved : BrokenRuleOrPolicyDomainEvent<MerchantRegistration, SimpleStringId>
{
    #region Constructor

    public MerchantRegistrationHasNotBeenApproved(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}