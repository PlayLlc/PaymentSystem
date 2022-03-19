using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface ICertificateAuthorityDatasetRepository
{
    public CertificateAuthorityDataset[] Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}