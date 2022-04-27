using System.Collections.Immutable;
using System.Numerics;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record ValueQualifiers : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, ValueQualifiers> _ValueObjectMap;
    public static readonly ValueQualifiers Empty = new();

    /// <value> decimal: 16; hexadecimal: 0x10 </value>
    public static readonly ValueQualifiers Amount;

    /// <value> decimal: 32; hexadecimal: 0x20 </value>
    public static readonly ValueQualifiers Balance;

    /// <value> decimal: 0; hexadecimal: 0x00 </value>
    public static readonly ValueQualifiers None;

    #endregion

    #region Constructor

    static ValueQualifiers()
    {
        const byte none = 0;
        const byte amount = 16;
        const byte balance = 32;

        None = new ValueQualifiers(none);
        Amount = new ValueQualifiers(amount);
        Balance = new ValueQualifiers(balance);

        _ValueObjectMap = new Dictionary<byte, ValueQualifiers> {{none, None}, {amount, Amount}, {balance, Balance}}.ToImmutableSortedDictionary();
    }

    private ValueQualifiers()
    { }

    private ValueQualifiers(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ValueQualifiers[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out ValueQualifiers? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator ValueQualifiers(byte value) => new(value);
    public static bool operator ==(ValueQualifiers left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ValueQualifiers right) => left == right._Value;
    public static explicit operator byte(ValueQualifiers value) => value._Value;
    public static explicit operator short(ValueQualifiers value) => value._Value;
    public static explicit operator ushort(ValueQualifiers value) => value._Value;
    public static explicit operator int(ValueQualifiers value) => value._Value;
    public static explicit operator uint(ValueQualifiers value) => value._Value;
    public static explicit operator long(ValueQualifiers value) => value._Value;
    public static explicit operator ulong(ValueQualifiers value) => value._Value;
    public static explicit operator BigInteger(ValueQualifiers value) => value._Value;
    public static bool operator !=(ValueQualifiers left, byte right) => !(left == right);
    public static bool operator !=(byte left, ValueQualifiers right) => !(left == right);

    #endregion
}