﻿namespace Play.Encryption.Certificates;

public class PublicKeyInfo
{
    #region Instance Values

    private readonly PublicKeyExponent _PublicKeyExponent;
    private readonly PublicKeyModulus _PublicKeyModulus;
    private readonly PublicKeyRemainder? _PublicKeyRemainder;

    #endregion

    #region Constructor

    public PublicKeyInfo(PublicKeyModulus publicKeyModulus, PublicKeyExponent publicKeyExponent, PublicKeyRemainder? publicKeyRemainder = null)
    {
        _PublicKeyModulus = publicKeyModulus;
        _PublicKeyExponent = publicKeyExponent;
        _PublicKeyRemainder = publicKeyRemainder;
    }

    #endregion

    #region Instance Members

    public bool IsPublicKeySplit() => _PublicKeyRemainder != null;
    public PublicKeyExponent GetPublicKeyExponent() => _PublicKeyExponent;
    public PublicKeyModulus GetPublicKeyModulus() => _PublicKeyModulus;

    public bool TryGetPublicKeyRemainder(out PublicKeyRemainder? result)
    {
        result = _PublicKeyRemainder;

        return result == null;
    }

    #endregion
}