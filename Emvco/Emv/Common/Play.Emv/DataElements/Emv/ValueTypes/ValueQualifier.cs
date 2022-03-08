using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Numerics;

using Play.Core.Extensions;

namespace Play.Emv.DataElements.Emv.ValueTypes;

public readonly struct ValueQualifier
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, ValueQualifier> _ValueObjectMap;

    /// <value> decimal: 16; hexadecimal: 0x10 </value>
    public static readonly ValueQualifier Amount;

    /// <value> decimal: 32; hexadecimal: 0x20 </value>
    public static readonly ValueQualifier Balance;

    /// <value> decimal: 0; hexadecimal: 0x00 </value>
    public static readonly ValueQualifier None;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static ValueQualifier()
    {
        const byte none = 0;
        const byte amount = 16;
        const byte balance = 32;

        None = new ValueQualifier(none);
        Amount = new ValueQualifier(amount);
        Balance = new ValueQualifier(balance);

        _ValueObjectMap = new Dictionary<byte, ValueQualifier> {{none, None}, {amount, Amount}, {balance, Balance}}
            .ToImmutableSortedDictionary();
    }

    private ValueQualifier(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static ValueQualifier Get(byte value)
    {
        const byte bitMask = 0b00001111;

        if (!_ValueObjectMap.ContainsKey(value.GetMaskedValue(bitMask)))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(ValueQualifier)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is ValueQualifier ValueQualifier && Equals(ValueQualifier);
    public bool Equals(ValueQualifier other) => _Value == other._Value;
    public bool Equals(ValueQualifier x, ValueQualifier y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;
    public override int GetHashCode() => 3001 * _Value.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ValueQualifier left, ValueQualifier right) => left._Value == right._Value;
    public static bool operator ==(ValueQualifier left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ValueQualifier right) => left == right._Value;
    public static explicit operator byte(ValueQualifier value) => value._Value;
    public static explicit operator short(ValueQualifier value) => value._Value;
    public static explicit operator ushort(ValueQualifier value) => value._Value;
    public static explicit operator int(ValueQualifier value) => value._Value;
    public static explicit operator uint(ValueQualifier value) => value._Value;
    public static explicit operator long(ValueQualifier value) => value._Value;
    public static explicit operator ulong(ValueQualifier value) => value._Value;
    public static explicit operator BigInteger(ValueQualifier value) => value._Value;
    public static bool operator !=(ValueQualifier left, ValueQualifier right) => !(left == right);
    public static bool operator !=(ValueQualifier left, byte right) => !(left == right);
    public static bool operator !=(byte left, ValueQualifier right) => !(left == right);

    #endregion
}