using Play.Emv.Ber.DataElements;

namespace Play.Emv.Terminal.Configuration.ApplicationDependent;

public class MerchantInformation
{
    #region Instance Values

    private readonly MerchantCategoryCode _MerchantCategoryCode;
    private readonly MerchantIdentifier _MerchantIdentifier;
    private readonly MerchantNameAndLocation _MerchantNameAndLocation;
    private readonly TerminalIdentification _TerminalIdentification;

    #endregion

    #region Constructor

    public MerchantInformation(
        TerminalIdentification terminalIdentification,
        MerchantNameAndLocation merchantNameAndLocation,
        MerchantIdentifier merchantIdentifier,
        MerchantCategoryCode merchantCategoryCode)
    {
        _TerminalIdentification = terminalIdentification;
        _MerchantNameAndLocation = merchantNameAndLocation;
        _MerchantIdentifier = merchantIdentifier;
        _MerchantCategoryCode = merchantCategoryCode;
    }

    #endregion
}