using System.Collections.Immutable;

using Play.Codecs;
using Play.Core;
using Play.Core.Extensions;

namespace Play.Encryption.Certificates;

/// <remarks>
///     Book 2 Section 5.2 lists the valid exponent values
/// </remarks>
public record PublicKeyExponents : EnumObject<uint>
{
    #region Static Metadata

    public static readonly PublicKeyExponents Empty = new();

    /// <value>Decimal: 3</value>
    /// >
    public static readonly PublicKeyExponents _3;

    /// <value>Decimal: 65537</value>
    /// >
    public static readonly PublicKeyExponents _65537;

    private static readonly ImmutableSortedDictionary<uint, PublicKeyExponents> _ValueObjectMap;

    #endregion

    #region Constructor

    public PublicKeyExponents()
    { }

    static PublicKeyExponents()
    {
        const uint __3 = 3;
        const uint __65537 = 65537;

        _3 = new PublicKeyExponents(__3);
        _65537 = new PublicKeyExponents(__65537);
        _ValueObjectMap = new Dictionary<uint, PublicKeyExponents> {{__3, _3}, {__65537, _65537}}.ToImmutableSortedDictionary();
    }

    private PublicKeyExponents(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PublicKeyExponents[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(uint value, out EnumObject<uint>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out PublicKeyExponents? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public byte[] Encode() => PlayCodec.UnsignedIntegerCodec.Encode(_Value);

    /// <exception cref="InvalidOperationException"></exception>
    public static PublicKeyExponents Create(uint value)
    {
        if (!_ValueObjectMap.TryGetValue(value, out PublicKeyExponents? result))
        {
            throw new InvalidOperationException(
                $"The argument {nameof(value)} is invalid. Valid values for this object are: {string.Join(',', _ValueObjectMap.Keys.Select(a => $" {a}"))}");
        }

        return result;
    }

    public int GetByteCount() => _Value.GetMostSignificantByte();

    #endregion

    #region Equality

    public bool Equals(PublicKeyExponents x, PublicKeyExponents y) => x.Equals(y);
    public int GetHashCode(PublicKeyExponents obj) => obj.GetHashCode();
    public override int GetHashCode() => unchecked(23561 * _Value.GetHashCode());

    #endregion

    #region Operator Overrides

    public static explicit operator uint(PublicKeyExponents value) => value._Value;
    public static bool operator >(PublicKeyExponents left, PublicKeyExponents right) => left._Value > right._Value;
    public static bool operator >=(PublicKeyExponents left, PublicKeyExponents right) => left._Value >= right._Value;
    public static bool operator <(PublicKeyExponents left, PublicKeyExponents right) => left._Value < right._Value;
    public static bool operator <=(PublicKeyExponents left, PublicKeyExponents right) => left._Value <= right._Value;

    #endregion
}