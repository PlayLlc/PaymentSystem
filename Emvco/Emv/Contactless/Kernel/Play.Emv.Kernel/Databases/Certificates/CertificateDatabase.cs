using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Security;
using Play.Emv.Security.Certificates;
using Play.Encryption.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases.Certificates;

public class CertificateDatabase : ICertificateDatabase
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<RegisteredApplicationProviderIndicator, CertificateAuthorityDataset> _Certificates;

    #endregion

    #region Constructor

    public CertificateDatabase(CertificateAuthorityDataset[] certificateAuthorityDataset)
    {
        _Certificates = certificateAuthorityDataset.ToImmutableSortedDictionary(a => a.GetRid(), b => b);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     IsRevoked
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="caPublicKeyIndex"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
    {
        if (!TryGet(rid, caPublicKeyIndex, out CaPublicKeyCertificate? result))
            return true;

        return result!.IsRevoked();
    }

    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CertificateSerialNumber serialNumber)
    {
        CertificateAuthorityDataset dataSet = _Certificates.FirstOrDefault(a => a.Key == rid).Value;

        if (!dataSet.TryGet(serialNumber, out CaPublicKeyCertificate? caPublicKeyCertificate))
            return true;

        return caPublicKeyCertificate!.IsRevoked();
    }

    public void PurgeRevokedCertificates()
    {
        for (int i = 0; i < _Certificates.Count; i++)
            _Certificates.ElementAt(i).Value.PurgeRevokedCertificates();
    }

    /// <summary>
    ///     TryGet
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="index"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        if (!_Certificates.TryGetValue(rid, out CertificateAuthorityDataset? dataset))
        {
            throw new
                InvalidOperationException($"The {nameof(CertificateDatabase)} does not have a {nameof(CertificateAuthorityDataset)} for the {nameof(RegisteredApplicationProviderIndicator)} value: [{rid}]");
        }

        return dataset.TryGet(index, out result);
    }

    #endregion
}