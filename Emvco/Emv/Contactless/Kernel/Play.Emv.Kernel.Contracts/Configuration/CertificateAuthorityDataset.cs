using System;
using System.Collections.Generic;
using System.Linq;

using Play.Core.Exceptions;
using Play.Emv.DataElements.Emv;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Contracts;

/// <summary>
///     The Certificate Authority Database will provide a dataset of Certificate Authority Public Keys based on the
///     RID for the current Kernel Session
/// </summary>
public class CertificateAuthorityDataset
{
    #region Instance Values

    protected readonly RegisteredApplicationProviderIndicator _RegisteredApplicationProviderIndicator;
    private readonly Dictionary<CaPublicKeyIndex, CaPublicKeyCertificate> _CaMap;

    #endregion

    #region Constructor

    protected CertificateAuthorityDataset(
        RegisteredApplicationProviderIndicator registeredApplicationProviderIndicator,
        CaPublicKeyCertificate[] certificateAuthorityPublicKeyCertificates)
    {
        CheckCore.ForNullOrEmptySequence(certificateAuthorityPublicKeyCertificates, nameof(certificateAuthorityPublicKeyCertificates));

        _RegisteredApplicationProviderIndicator = registeredApplicationProviderIndicator;
        _CaMap = certificateAuthorityPublicKeyCertificates.ToDictionary(a => a.GetCaPublicKeyIndex(), b => b);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Get
    /// </summary>
    /// <param name="index"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryGet(CaPublicKeyIndex index, out CaPublicKeyCertificate? result) => _CaMap.TryGetValue(index, out result);

    /// <returns>
    ///     <see cref="RegisteredApplicationProviderIndicator" />
    /// </returns>
    public RegisteredApplicationProviderIndicator GetRid() => _RegisteredApplicationProviderIndicator;

    // TODO: not sure why they mentioned the serial number in the book
    public bool IsRevoked(CaPublicKeyIndex caPublicKeyIndex /*, CertificateSerialNumber serialNumber*/)
    {
        if (!_CaMap.TryGetValue(caPublicKeyIndex, out CaPublicKeyCertificate? result))
            return true;

        return result.IsRevoked() || result.IsExpired();
    }

    public void PurgeRevokedCertificates()
    {
        for (int i = 0; i < _CaMap.Count; i++)
        {
            if (_CaMap.ElementAt(i).Value.IsRevoked())
                _CaMap.Remove(_CaMap.ElementAt(i).Key);
        }
    }

    #endregion
}