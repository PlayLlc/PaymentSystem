using System.Collections.Generic;
using System.Linq;

using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel2.Databases;

public class Kernel2CertificateAuthorityDatabase : ICertificateAuthorityDatabase
{
    #region Instance Values

    private readonly Dictionary<RegisteredApplicationProviderIndicator, CertificateAuthorityDataset> _CertificateMap;

    #endregion

    #region Constructor

    public Kernel2CertificateAuthorityDatabase(CertificateAuthorityDataset[] certificateAuthorityDatasets)
    {
        _CertificateMap = certificateAuthorityDatasets.ToDictionary(a => a.GetRid(), b => b);
    }

    #endregion

    #region Instance Members

    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex) =>
        _CertificateMap[rid]!.IsRevoked(caPublicKeyIndex);

    public void PurgeRevokedCertificates()
    {
        for (int i = 0; i < _CertificateMap.Count; i++)
            _CertificateMap.ElementAt(i).Value.PurgeRevokedCertificates();
    }

    /// <summary>
    ///     TryGet
    /// </summary>
    /// <param name="rid"></param>
    /// <param name="index"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        if (!_CertificateMap.TryGetValue(rid, out CertificateAuthorityDataset? caDataset))

        {
            result = null;

            return false;
        }

        if (caDataset.Get(index).IsRevoked())
        {
            result = null;

            return false;
        }

        result = caDataset.Get(index);

        return true;
    }

    #endregion
}