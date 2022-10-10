using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

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