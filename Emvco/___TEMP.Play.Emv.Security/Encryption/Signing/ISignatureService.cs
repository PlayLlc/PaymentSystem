using System;

using Play.Emv.DataElements;
using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Contracts;

namespace Play.Emv.Security.Encryption.Signing;

internal interface ISignatureService
{
    #region Instance Members

    public DecodedSignature Decrypt(ReadOnlySpan<byte> signature, PublicKeyCertificate publicKeyCertificate);
    public bool IsSignatureValid(HashAlgorithmIndicator hashAlgorithmIndicator, ReadOnlySpan<byte> message, DecodedSignature signature);
    public byte[] Sign(ReadOnlySpan<byte> message, PublicKeyCertificate publicKeyCertificate);

    #endregion
}