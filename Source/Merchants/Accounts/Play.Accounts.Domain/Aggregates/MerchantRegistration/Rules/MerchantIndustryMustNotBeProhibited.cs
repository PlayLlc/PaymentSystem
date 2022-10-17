using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

public class MerchantIndustryMustNotBeProhibited : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly bool _IsProhibited;
    public override string Message => "Merchant cannot be registered if their industry is prohibited";

    #endregion

    #region Constructor

    public MerchantIndustryMustNotBeProhibited(MerchantCategoryCode merchantCategoryCode, IUnderwriteMerchants underwritingService)
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