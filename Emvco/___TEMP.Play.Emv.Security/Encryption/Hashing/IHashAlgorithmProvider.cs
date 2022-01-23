using System;

using Play.Emv.DataElements.CertificateAuthority;
using Play.Emv.Security.Contracts;

namespace Play.Emv.Security.Encryption;

internal interface IHashAlgorithmProvider
{
    #region Instance Members

    public Hash Generate(ReadOnlySpan<byte> clearText, HashAlgorithmIndicator hashAlgorithmIndicator);

    #endregion
}