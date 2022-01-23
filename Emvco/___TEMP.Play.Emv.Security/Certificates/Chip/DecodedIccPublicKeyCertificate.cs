using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Globalization.Time;

namespace ___TEMP.Play.Emv.Security.Certificates.Chip;

public class DecodedIccPublicKeyCertificate : PublicKeyCertificate
{
    #region Static Metadata

    private static readonly CertificateFormat _CertificateFormat = CertificateFormat.Icc;

    #endregion

    #region Instance Values

    private readonly PrimaryAccountNumber _PrimaryAccountNumber;

    #endregion

    #region Constructor

    public DecodedIccPublicKeyCertificate(
        PrimaryAccountNumber applicationNumber,
        DateRange validityPeriod,
        CertificateSerialNumber certificateSerialNumber,
        HashAlgorithmIndicator hashAlgorithmIndicator,
        PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator,
        PublicKeyInfo publicKeyInfo) : base(certificateSerialNumber, hashAlgorithmIndicator, publicKeyAlgorithmIndicator, validityPeriod,
                                            publicKeyInfo)
    {
        _PrimaryAccountNumber = applicationNumber;
    }

    #endregion

    #region Instance Members

    public CertificateFormat GetCertificateFormat()
    {
        return _CertificateFormat;
    }

    public PrimaryAccountNumber GetPrimaryAccountNumber()
    {
        return _PrimaryAccountNumber;
    }

    #endregion
}