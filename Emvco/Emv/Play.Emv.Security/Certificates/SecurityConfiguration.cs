﻿using System.Collections.Immutable;
using System.Linq;

using Play.Core.Exceptions;

namespace Play.Emv.Security.Certificates;

public class SecurityConfiguration
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<CaPublicKeyCertificateIdentifier, CaPublicKeyCertificate> _CertificateMap;

    #endregion

    #region Constructor

    public SecurityConfiguration(CaPublicKeyCertificate[] certificateAuthorityPublicKeyCertificates)
    {
        CheckCore.ForNullOrEmptySequence(certificateAuthorityPublicKeyCertificates, nameof(certificateAuthorityPublicKeyCertificates));
        _CertificateMap = certificateAuthorityPublicKeyCertificates.ToImmutableSortedDictionary(a => a.GetId(), b => b);
    }

    #endregion

    #region Instance Members

    public CaPublicKeyCertificate Get(CaPublicKeyCertificateIdentifier id)
    {
        return _CertificateMap[id];
    }

    public CaPublicKeyCertificate[] GetActiveCertificates()
    {
        return _CertificateMap.Values.Where(a => !a.IsRevoked()).ToArray();
    }

    public CaPublicKeyCertificate[] GetRevokedCertificates()
    {
        return _CertificateMap.Values.Where(a => a.IsRevoked()).ToArray();
    }

    public bool IsRevoked(CaPublicKeyCertificateIdentifier caPublicKeyCertificateIdentifier)
    {
        return _CertificateMap.ContainsKey(caPublicKeyCertificateIdentifier);
    }

    #endregion
}