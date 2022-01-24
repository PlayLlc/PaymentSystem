using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Play.Encryption.Certificates;

public class PublicKeyAlgorithmIndicator : IEqualityComparer<PublicKeyAlgorithmIndicator>, IEquatable<PublicKeyAlgorithmIndicator>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, PublicKeyAlgorithmIndicator> _ValueObjectMap;
    public static readonly PublicKeyAlgorithmIndicator Rsa;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static PublicKeyAlgorithmIndicator()
    {
        const byte rsa = 0x01;

        Rsa = new PublicKeyAlgorithmIndicator(rsa);
        _ValueObjectMap = new Dictionary<byte, PublicKeyAlgorithmIndicator> {{rsa, Rsa}}.ToImmutableSortedDictionary();
    }

    private PublicKeyAlgorithmIndicator(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static bool Exists(byte value) => _ValueObjectMap.Keys.Contains(value);

    public static PublicKeyAlgorithmIndicator Get(byte value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out PublicKeyAlgorithmIndicator? result))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(PublicKeyAlgorithmIndicator)} could be retrieved because the argument provided does not match a definition value");
        }

        return result!;
    }

    #endregion

    #region Equality

    public bool Equals([AllowNull] PublicKeyAlgorithmIndicator other)
    {
        if (other == null)
            return false;

        return _Value == other._Value;
    }

    public bool Equals([AllowNull] PublicKeyAlgorithmIndicator x, [AllowNull] PublicKeyAlgorithmIndicator y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override bool Equals([AllowNull] object obj) =>
        obj is PublicKeyAlgorithmIndicator publicKeyAlgorithmIndicator && Equals(publicKeyAlgorithmIndicator);

    public int GetHashCode(PublicKeyAlgorithmIndicator obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(297581 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static bool operator ==(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value == right._Value;
    public static explicit operator byte(PublicKeyAlgorithmIndicator value) => value._Value;
    public static bool operator >(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value > right._Value;
    public static bool operator >=(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value >= right._Value;
    public static bool operator !=(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => !(left == right);
    public static bool operator <(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value < right._Value;
    public static bool operator <=(PublicKeyAlgorithmIndicator left, PublicKeyAlgorithmIndicator right) => left._Value <= right._Value;

    #endregion
}