using Play.Ber.DataObjects;
using Play.Emv.Configuration;
using Play.Emv.DataElements;

namespace Play.Emv.Reader.Services;

internal abstract class ReaderDatabase
{
    #region Instance Members

    public abstract KernelConfiguration GetKernelConfiguration(KernelId kernelId);
    public abstract TagLengthValue[] GetPersistentKernelValues(KernelId kernelId);
    public abstract CertificateAuthorityDataset[] GetCertificateAuthorityDatasets(KernelId kernelId);
    public abstract PcdProtocolConfiguration GetPcdProtocolConfiguration(string deviceId);
    public abstract TransactionProfile[] GetTransactionProfiles();
    public abstract DisplayMessages GetDisplayMessages(LanguagePreference languagePreference);

    #endregion
}