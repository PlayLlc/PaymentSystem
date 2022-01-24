using System;
using System.Collections.Immutable;
using System.Linq;

using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public class KernelCertificateDatabase : IKernelCertificateDatabase
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<RegisteredApplicationProviderIndicator, CertificateAuthorityDataset> _Certificates;

    #endregion

    #region Constructor

    public KernelCertificateDatabase(CertificateAuthorityDataset[] certificateAuthorityDataset)
    {
        _Certificates = certificateAuthorityDataset.ToImmutableSortedDictionary(a => a.GetRid(), b => b);
    }

    #endregion

    #region Instance Members

    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
    {
        if (!TryGet(rid, caPublicKeyIndex, out CaPublicKeyCertificate? result))
            return true;

        return result!.IsRevoked();
    }

    public void PurgeRevokedCertificates()
    {
        for (int i = 0; i < _Certificates.Count; i++)
            _Certificates.ElementAt(i).Value.PurgeRevokedCertificates();
    }

    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        if (!_Certificates.TryGetValue(rid, out CertificateAuthorityDataset? dataset))
        {
            throw new
                InvalidOperationException($"The {nameof(KernelCertificateDatabase)} does not have a {nameof(CertificateAuthorityDataset)} for the {nameof(RegisteredApplicationProviderIndicator)} value: [{rid}]");
        }

        return dataset.TryGet(index, out result);
    }

    #endregion
}