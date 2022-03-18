using System.Collections.Immutable;

using Play.Codecs;
using Play.Core.Extensions;

namespace Play.Encryption.Certificates;

/// <remarks>
///     Book 2 Section 5.2 lists the valid exponent values
/// </remarks>
public readonly record struct PublicKeyExponent
{
    #region Static Metadata

    /// <value>Decimal: 3</value>
    /// >
    public static readonly PublicKeyExponent _3;

    /// <value>Decimal: 65537</value>
    /// >
    public static readonly PublicKeyExponent _65537;

    private static readonly ImmutableSortedDictionary<uint, PublicKeyExponent> _ValueObjectMap;

    #endregion

    #region Instance Values

    private readonly uint _Value;

    #endregion

    #region Constructor

    static PublicKeyExponent()
    {
        const uint __3 = 3;
        const uint __65537 = 65537;

        _3 = new PublicKeyExponent(__3);
        _65537 = new PublicKeyExponent(__65537);
        _ValueObjectMap = new Dictionary<uint, PublicKeyExponent> {{__3, _3}, {__65537, _65537}}.ToImmutableSortedDictionary();
    }

    private PublicKeyExponent(uint value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] Encode() => PlayCodec.UnsignedIntegerCodec.Encode(_Value);

    /// <exception cref="InvalidOperationException"></exception>
    public static PublicKeyExponent Create(uint value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out PublicKeyExponent result))
        {
            throw new
                InvalidOperationException($"The argument {nameof(value)} is invalid. Valid values for this object are: {string.Join(',', _ValueObjectMap.Keys.Select(a => $" {a}"))}");
        }

        return result;
    }

    public int GetByteCount() => _Value.GetMostSignificantByte();

    #endregion

    #region Equality

    public bool Equals(PublicKeyExponent x, PublicKeyExponent y) => x.Equals(y);
    public int GetHashCode(PublicKeyExponent obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(23561 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static explicit operator uint(PublicKeyExponent value) => value._Value;
    public static bool operator >(PublicKeyExponent left, PublicKeyExponent right) => left._Value > right._Value;
    public static bool operator >=(PublicKeyExponent left, PublicKeyExponent right) => left._Value >= right._Value;
    public static bool operator <(PublicKeyExponent left, PublicKeyExponent right) => left._Value < right._Value;
    public static bool operator <=(PublicKeyExponent left, PublicKeyExponent right) => left._Value <= right._Value;

    #endregion
}