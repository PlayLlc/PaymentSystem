using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationHasExpired : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Instance Values

    public readonly string MerchantRegistrationId;

    #endregion

    #region Constructor

    public MerchantRegistrationHasExpired(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    {
        MerchantRegistrationId = merchantRegistration.GetId();
    }

    #endregion
}