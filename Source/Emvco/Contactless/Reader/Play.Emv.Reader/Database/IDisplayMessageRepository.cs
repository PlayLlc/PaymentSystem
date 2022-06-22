using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Contracts;

namespace Play.Emv.Reader.Database;

public interface IDisplayMessageRepository
{
    #region Instance Members

    public DisplayMessages Get(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference);

    #endregion
}