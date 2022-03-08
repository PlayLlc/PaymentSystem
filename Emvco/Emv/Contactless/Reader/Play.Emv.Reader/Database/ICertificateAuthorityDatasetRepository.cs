using Play.Emv.DataElements.Emv.Primitives.Issuer;
using Play.Emv.DataElements.Emv.Primitives.Merchant;
using Play.Emv.DataElements.Emv.Primitives.Terminal;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader.Database;

public interface ICertificateAuthorityDatasetRepository
{
    public CertificateAuthorityDataset[] Get(
        IssuerIdentificationNumber issuerIdentificationNumber,
        MerchantIdentifier merchantIdentifier,
        TerminalIdentification terminalIdentification);
}