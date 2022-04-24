using System.Collections.Immutable;

using Play.Core;

namespace Play.Encryption.Certificates;

public record PublicKeyAlgorithmIndicator : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, PublicKeyAlgorithmIndicator> _ValueObjectMap;
    public static readonly PublicKeyAlgorithmIndicator Empty = new();
    public static readonly PublicKeyAlgorithmIndicator Rsa;

    #endregion

    #region Constructor

    public PublicKeyAlgorithmIndicator() : base()
    { }

    static PublicKeyAlgorithmIndicator()
    {
        const byte rsa = 0x01;

        Rsa = new PublicKeyAlgorithmIndicator(rsa);
        _ValueObjectMap = new Dictionary<byte, PublicKeyAlgorithmIndicator> {{rsa, Rsa}}.ToImmutableSortedDictionary();
    }

    private PublicKeyAlgorithmIndicator(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PublicKeyAlgorithmIndicator[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out PublicKeyAlgorithmIndicator? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public bool Exists(byte value) => _ValueObjectMap.ContainsKey(value);

    #endregion

    #region Equality

    public bool Equals(PublicKeyAlgorithmIndicator? x, PublicKeyAlgorithmIndicator? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PublicKeyAlgorithmIndicator obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(297581 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static explicit operator byte(PublicKeyAlgorithmIndicator value) => value._Value;
    public static bool operator >(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value > right._Value;
    public static bool operator >=(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value >= right._Value;
    public static bool operator <(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value < right._Value;
    public static bool operator <=(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value <= right._Value;

    #endregion
}