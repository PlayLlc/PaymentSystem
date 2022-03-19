using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Reader.Database;

public interface IPcdProtocolConfigurationRepository
{
    public PcdProtocolConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}