using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv.Primitives.Issuer;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;
using Play.Emv.Display.Contracts;

namespace Play.Emv.Reader.Database;

public interface IDisplayMessageRepository
{
    public DisplayMessages Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification,
        LanguagePreference languagePreference);
}