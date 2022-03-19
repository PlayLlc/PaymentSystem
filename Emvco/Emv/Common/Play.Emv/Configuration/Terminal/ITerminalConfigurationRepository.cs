using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

internal interface ITerminalConfigurationRepository
{
    public TerminalConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}