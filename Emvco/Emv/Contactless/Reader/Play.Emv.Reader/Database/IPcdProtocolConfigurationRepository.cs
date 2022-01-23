using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public interface IPcdProtocolConfigurationRepository
{
    public PcdProtocolConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}