using Play.Emv.DataElements;

namespace Play.Emv.Pos.Merchant;

/// <summary>
///     The values that have been assigned and configured for the merchant
/// </summary>
public abstract class MerchantConfiguration
{
    #region Instance Values

    private readonly MerchantCategoryCode _MerchantCategoryCode;
    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly MerchantNameAndLocation _MerchantNameAndLocation;

    #endregion

    #region Constructor

    public MerchantConfiguration(
        MerchantIdentifier merchantIdentifier,
        MerchantCategoryCode merchantCategoryCode,
        MerchantNameAndLocation merchantNameAndLocation)
    {
        _MerchantIdentifier = merchantIdentifier;
        _MerchantCategoryCode = merchantCategoryCode;
        _MerchantNameAndLocation = merchantNameAndLocation;
    }

    #endregion
}