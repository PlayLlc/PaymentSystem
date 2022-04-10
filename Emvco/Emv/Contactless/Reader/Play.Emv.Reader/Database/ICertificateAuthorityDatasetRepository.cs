using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface ICertificateAuthorityDatasetRepository
{
    #region Instance Members

    public CertificateAuthorityDataset[] Get(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);

    #endregion
}