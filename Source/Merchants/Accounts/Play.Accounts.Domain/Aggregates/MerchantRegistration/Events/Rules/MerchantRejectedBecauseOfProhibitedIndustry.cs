using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRejectedBecauseOfProhibitedIndustry : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Instance Values

    public readonly string MerchantRegistrationId;

    #endregion

    #region Constructor

    public MerchantRejectedBecauseOfProhibitedIndustry(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    {
        MerchantRegistrationId = merchantRegistration.GetId();
    }

    #endregion
}