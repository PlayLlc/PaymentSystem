using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record MerchantIsProhibited : BrokenBusinessRuleDomainEvent<Merchant, string>
{
    #region Instance Values

    public readonly Merchant MerchantRegistration;

    #endregion

    #region Constructor

    public MerchantIsProhibited(Merchant merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    {
        MerchantRegistration = merchantRegistration;
    }

    #endregion
}