using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Reader.Database;

public interface IReaderRepository
{
    #region Instance Members

    public PrimitiveValue[] GetReaderConfiguration(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification);

    #endregion
}