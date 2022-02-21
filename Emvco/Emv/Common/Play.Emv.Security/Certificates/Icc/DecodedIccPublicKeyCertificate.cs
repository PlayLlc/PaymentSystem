﻿using Play.Encryption.Certificates;
using Play.Encryption.Hashing;
using Play.Globalization.Time;

using PrimaryAccountNumber = Play.Emv.DataElements.Interchange.PrimaryAccountNumber;

namespace Play.Emv.Security.Certificates.Icc;

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

    public CertificateFormat GetCertificateFormat() => _CertificateFormat;
    public PrimaryAccountNumber GetPrimaryAccountNumber() => _PrimaryAccountNumber;

    #endregion
}