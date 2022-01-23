using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public interface IDisplayMessageRepository
{
    public DisplayMessages Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference);
}