using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;

public record MerchantHasAlreadyBeenRegistered : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantHasAlreadyBeenRegistered(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}