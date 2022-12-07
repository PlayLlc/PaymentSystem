using System.Collections.Immutable;

using Play.Core;

namespace Play.Encryption.Certificates;

public record PublicKeyAlgorithmIndicators : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, PublicKeyAlgorithmIndicators> _ValueObjectMap;
    public static readonly PublicKeyAlgorithmIndicators Empty = new();
    public static readonly PublicKeyAlgorithmIndicators Rsa;

    #endregion

    #region Constructor

    public PublicKeyAlgorithmIndicators()
    { }

    static PublicKeyAlgorithmIndicators()
    {
        const byte rsa = 0x01;

        Rsa = new PublicKeyAlgorithmIndicators(rsa);
        _ValueObjectMap = new Dictionary<byte, PublicKeyAlgorithmIndicators> {{rsa, Rsa}}.ToImmutableSortedDictionary();
    }

    private PublicKeyAlgorithmIndicators(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PublicKeyAlgorithmIndicators[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out PublicKeyAlgorithmIndicators? enumResult))
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

    public bool Equals(PublicKeyAlgorithmIndicators? x, PublicKeyAlgorithmIndicators? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PublicKeyAlgorithmIndicators obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(297581 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static explicit operator byte(PublicKeyAlgorithmIndicators value) => value._Value;
    public static bool operator >(PublicKeyAlgorithmIndicators left, PublicKeyAlgorithmIndicators right) => left._Value > right._Value;
    public static bool operator >=(PublicKeyAlgorithmIndicators left, PublicKeyAlgorithmIndicators right) => left._Value >= right._Value;
    public static bool operator <(PublicKeyAlgorithmIndicators left, PublicKeyAlgorithmIndicators right) => left._Value < right._Value;
    public static bool operator <=(PublicKeyAlgorithmIndicators left, PublicKeyAlgorithmIndicators right) => left._Value <= right._Value;

    #endregion
}