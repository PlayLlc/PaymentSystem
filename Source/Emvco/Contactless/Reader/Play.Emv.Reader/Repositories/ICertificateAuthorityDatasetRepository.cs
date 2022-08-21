using System.Collections.Generic;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;

namespace Play.Emv.Reader;

public interface ICertificateAuthorityDatasetRepository
{
    #region Instance Members

    public Dictionary<KernelId, CertificateAuthorityDataset[]> GetCertificateAuthorityDatasets(
        IssuerIdentificationNumber issuerIdentificationNumber, MerchantIdentifier merchantIdentifier, TerminalIdentification terminalIdentification);

    #endregion
}