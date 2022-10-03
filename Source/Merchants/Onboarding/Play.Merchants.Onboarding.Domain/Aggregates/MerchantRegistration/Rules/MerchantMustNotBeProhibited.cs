using Play.Domain;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Services;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Rules;

internal class MerchantMustNotBeProhibited : IBusinessRule
{
    #region Instance Values

    private readonly Name _Name;
    private readonly Address _Address;
    private readonly IUnderwriteMerchants _UnderwritingService;

    public string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public MerchantMustNotBeProhibited(Name companyName, Address companyAddress, IUnderwriteMerchants underwritingService)
    {
        _Name = companyName;
        _Address = companyAddress;
        _UnderwritingService = underwritingService;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _UnderwritingService.IsMerchantProhibited(_Name, _Address);
    }

    #endregion
}