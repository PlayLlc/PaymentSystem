using Play.Emv.Ber.DataElements;

namespace Play.Emv.Configuration;

internal interface ITerminalConfigurationRepository
{
    public TerminalConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}