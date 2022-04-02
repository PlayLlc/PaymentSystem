using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Encryption.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Security;

public interface ICertificateDatabase
{
    #region Instance Members

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex);

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CertificateSerialNumber serialNumber);

    /// <summary>
    ///     Updates the <see cref="ICertificateDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
    ///     that has expired since the last time they were checked
    /// </summary>
    public void PurgeRevokedCertificates();

    /// <summary>
    ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
    ///     <param name="rid" />
    ///     and
    ///     <param name="index"></param>
    ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none can be found then the return value will
    ///     be false
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result);

    #endregion
}