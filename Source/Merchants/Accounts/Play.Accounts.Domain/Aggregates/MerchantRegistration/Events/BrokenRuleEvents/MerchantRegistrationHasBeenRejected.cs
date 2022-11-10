using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantRegistrationHasBeenRejected : BrokenBusinessRuleDomainEvent<MerchantRegistration, string>
{
    #region Instance Values

    public readonly MerchantRegistration MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantRegistrationHasBeenRejected(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}