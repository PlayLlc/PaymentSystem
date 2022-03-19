using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;

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