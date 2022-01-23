using System;
using System.Collections.Immutable;

using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public interface ICertificateAuthorityDatabase
{
    #region Instance Members

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex);

    /// <summary>
    ///     Updates the <see cref="ICertificateAuthorityDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
    ///     that has expired since the last time they were checked
    /// </summary>
    public void PurgeRevokedCertificates();

    /// <summary>
    ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
    ///     <param name="rid" />
    ///     and
    ///     <param name="index"></param>
    ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none
    ///     can be found then the return value will be false
    /// </summary>
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result);

    #endregion
}

public class CertificateAuthorityDatabase : ICertificateAuthorityDatabase
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<CaPublicKeyCertificateIdentifier, CaPublicKeyCertificate> _CertificateMap;

    #endregion

    #region Instance Members

    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
    {
        throw new NotImplementedException();
    }

    public void PurgeRevokedCertificates()
    {
        throw new NotImplementedException();
    }

    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        throw new NotImplementedException();
    }

    #endregion
}