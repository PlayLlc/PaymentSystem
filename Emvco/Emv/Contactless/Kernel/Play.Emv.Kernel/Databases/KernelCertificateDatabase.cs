using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public class KernelCertificateDatabase : IKernelCertificateDatabase
{
    #region Instance Values

    private readonly CertificateAuthorityDataset _Certificates;

    #endregion

    #region Constructor

    public KernelCertificateDatabase(CertificateAuthorityDataset certificateAuthorityDataset)
    {
        _Certificates = certificateAuthorityDataset;
    }

    #endregion

    #region Instance Members

    public bool IsRevoked(CaPublicKeyIndex caPublicKeyIndex) => _Certificates.IsRevoked(caPublicKeyIndex);
    public void PurgeRevokedCertificates() => _Certificates.PurgeRevokedCertificates();
    public bool TryGet(CaPublicKeyIndex index, out CaPublicKeyCertificate? result) => _Certificates.TryGet(index, out result);

    /// <returns>
    ///     <see cref="RegisteredApplicationProviderIndicator" />
    /// </returns>
    public RegisteredApplicationProviderIndicator GetRid() => _Certificates.GetRid();

    #endregion
}