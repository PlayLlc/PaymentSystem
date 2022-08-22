namespace Play.Encryption.Certificates;

public class PublicKeyInfo
{
    #region Instance Values

    private readonly PublicKeyExponents _PublicKeyExponents;
    private readonly PublicKeyModulus _PublicKeyModulus;
    private readonly PublicKeyRemainder? _PublicKeyRemainder;

    #endregion

    #region Constructor

    public PublicKeyInfo(PublicKeyModulus publicKeyModulus, PublicKeyExponents publicKeyExponents)
    {
        _PublicKeyModulus = publicKeyModulus;
        _PublicKeyExponents = publicKeyExponents;

        if (publicKeyModulus.GetByteCount() > 36)
            _PublicKeyRemainder = new PublicKeyRemainder(publicKeyModulus.AsByteArray()[35..]);
        else
            _PublicKeyRemainder = new PublicKeyRemainder(0);
    }

    public PublicKeyInfo(PublicKeyModulus publicKeyModulus, PublicKeyExponents publicKeyExponents, PublicKeyRemainder? publicKeyRemainder = null)
    {
        _PublicKeyModulus = publicKeyModulus;
        _PublicKeyExponents = publicKeyExponents;
        _PublicKeyRemainder = publicKeyRemainder;
    }

    #endregion

    #region Instance Members

    public bool IsPublicKeySplit() => _PublicKeyRemainder != null;
    public PublicKeyExponents GetPublicKeyExponent() => _PublicKeyExponents;
    public PublicKeyModulus GetPublicKeyModulus() => _PublicKeyModulus;

    public bool TryGetPublicKeyRemainder(out PublicKeyRemainder? result)
    {
        result = _PublicKeyRemainder;

        return result == null;
    }

    #endregion
}