using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantRejectedBecauseTheRegistrationPeriodExpired : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRejectedBecauseTheRegistrationPeriodExpired(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}