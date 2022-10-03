using Play.Domain;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.Services;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Rules;

internal class MerchantIndustryMustNotBeProhibited : IBusinessRule
{
    #region Instance Values

    private readonly MerchantCategoryCodes _MerchantCategoryCode;
    private readonly IUnderwriteMerchants _UnderwritingService;

    public string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public MerchantIndustryMustNotBeProhibited(MerchantCategoryCodes categoryCode, IUnderwriteMerchants underwritingService)
    {
        _MerchantCategoryCode = categoryCode;
        _UnderwritingService = underwritingService;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _UnderwritingService.IsIndustryProhibited(_MerchantCategoryCode);
    }

    #endregion
}