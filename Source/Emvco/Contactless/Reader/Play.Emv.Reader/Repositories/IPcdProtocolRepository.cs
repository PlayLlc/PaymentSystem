using Play.Emv.Ber.DataElements;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Reader;

public interface IPcdProtocolRepository
{
    #region Instance Members

    public PcdProtocolConfiguration Get(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification);

    #endregion
}