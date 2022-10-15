using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantRegistrationHasNotBeenVerified : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRegistrationHasNotBeenVerified(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}