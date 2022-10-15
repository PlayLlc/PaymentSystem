using Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public class MerchantIndustryMustNotBeProhibited : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly bool _IsProhibited;
    public override string Message => "Merchant cannot be registered if their industry is prohibited";

    #endregion

    #region Constructor

    public MerchantIndustryMustNotBeProhibited(MerchantCategoryCodes merchantCategoryCode, IUnderwriteMerchants underwritingService)
    {
        _IsProhibited = underwritingService.IsIndustryProhibited(merchantCategoryCode);
        ;
    }

    #endregion

    #region Instance Members

    public override MerchantRejectedBecauseOfProhibitedIndustry CreateBusinessRuleViolationDomainEvent(MerchantRegistration aggregate)
    {
        return new MerchantRejectedBecauseOfProhibitedIndustry(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsProhibited;
    }

    #endregion
}