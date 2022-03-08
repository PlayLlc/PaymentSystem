using Play.Emv.DataElements.Emv.Primitives.Issuer;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;

namespace Play.Emv.Configuration;

internal interface ITerminalConfigurationRepository
{
    public TerminalConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}