using Play.Emv.DataElements;

namespace Play.Emv.Configuration;

public interface ICertificateAuthorityDatasetRepository
{
    public CertificateAuthorityDataset[] Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}