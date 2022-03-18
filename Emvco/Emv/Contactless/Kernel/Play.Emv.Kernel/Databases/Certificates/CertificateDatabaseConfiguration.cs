using System.Collections.Immutable;
using System.Linq;

using Play.Core.Exceptions;
using Play.Emv.Security.Certificates;

namespace Play.Emv.Kernel.Databases;

public class CertificateDatabaseConfiguration
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<CaPublicKeyCertificateIdentifier, CaPublicKeyCertificate> _CertificateMap;

    #endregion

    #region Constructor

    public CertificateDatabaseConfiguration(CaPublicKeyCertificate[] certificateAuthorityPublicKeyCertificates)
    {
        CheckCore.ForNullOrEmptySequence(certificateAuthorityPublicKeyCertificates, nameof(certificateAuthorityPublicKeyCertificates));
        _CertificateMap = certificateAuthorityPublicKeyCertificates.ToImmutableSortedDictionary(a => a.GetId(), b => b);
    }

    #endregion

    #region Instance Members

    public CaPublicKeyCertificate Get(CaPublicKeyCertificateIdentifier id) => _CertificateMap[id];

    public CaPublicKeyCertificate[] GetActiveCertificates()
    {
        return _CertificateMap.Values.Where(a => !a.IsRevoked()).ToArray();
    }

    public CaPublicKeyCertificate[] GetRevokedCertificates()
    {
        return _CertificateMap.Values.Where(a => a.IsRevoked()).ToArray();
    }

    public bool IsRevoked(CaPublicKeyCertificateIdentifier caPublicKeyCertificateIdentifier) =>
        _CertificateMap.ContainsKey(caPublicKeyCertificateIdentifier);

    #endregion
}