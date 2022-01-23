namespace Play.Emv.DataElements.CertificateAuthority;

public readonly struct PublicKeyRemainder
{
    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public PublicKeyRemainder(byte[] value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value;
    public Span<byte> AsSpan() => _Value;
    public int GetByteCount() => _Value.Length;
    public bool IsEmpty() => _Value.Length == 0;

    #endregion

    //#region IEqualityComparer

    //public bool Equals([AllowNull] PublicKeyRemainder x, [AllowNull] PublicKeyRemainder y)
    //{
    //    return x.Equals(y);
    //}

    //public int GetHashCode(PublicKeyRemainder obj) => obj.GetHashCode();

    //#endregion

    //#region Object Overrides

    //public override bool Equals([AllowNull] object obj) => obj is PublicKeyRemainder publicKeyRemainder && Equals(publicKeyRemainder);

    //public override int GetHashCode()
    //{
    //    return unchecked(85691 * _Value.GetHashCode());
    //}

    //#endregion

    //#region Operator Overrides

    //public static bool operator ==(PublicKeyRemainder left, PublicKeyRemainder right) => left._Value == right._Value;
    //public static bool operator !=(PublicKeyRemainder left, PublicKeyRemainder right) => !(left == right);

    //public static bool operator <(PublicKeyRemainder left, PublicKeyRemainder right) => left._Value < right._Value;
    //public static bool operator <=(PublicKeyRemainder left, PublicKeyRemainder right) => left._Value <= right._Value;
    //public static bool operator >(PublicKeyRemainder left, PublicKeyRemainder right) => left._Value > right._Value;
    //public static bool operator >=(PublicKeyRemainder left, PublicKeyRemainder right) => left._Value >= right._Value;

    //#endregion
}