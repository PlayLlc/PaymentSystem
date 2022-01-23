using System;
using System.Collections.Generic;
using System.Linq;

using Play.Core.Exceptions;
using Play.Emv.DataElements;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Configuration;

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
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public CaPublicKeyCertificate Get(CaPublicKeyIndex index)
    {
        if (!_CaMap.TryGetValue(index, out CaPublicKeyCertificate? result))
            throw new InvalidOperationException($"The argument {nameof(index)} had a value of {(byte) index} which was not available");

        return result!;
    }

    /// <returns>
    ///     <see cref="RegisteredApplicationProviderIndicator" />
    /// </returns>
    public RegisteredApplicationProviderIndicator GetRid()
    {
        return _RegisteredApplicationProviderIndicator;
    }

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