using System;
using System.Security.Cryptography;

using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Contracts;

namespace Play.Emv.Security.Encryption;

internal class Sha1HashGenerator : IHashGenerator
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText) => new(SHA1.HashData(clearText));
    public HashAlgorithmIndicator GetHashAlgorithmIndicator() => HashAlgorithmIndicator.Sha1;

    #endregion
}