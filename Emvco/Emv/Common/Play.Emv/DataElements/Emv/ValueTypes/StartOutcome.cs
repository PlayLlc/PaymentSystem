using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Emv.DataElements.Emv;

public readonly struct StartOutcome
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, StartOutcome> _ValueObjectMap;

    /// <value>Decimal: 0; HexadecimalCodec: 0x0</value>
    public static readonly StartOutcome A;

    /// <value>Decimal: 16; HexadecimalCodec: 0x10</value>
    public static readonly StartOutcome B;

    /// <value>Decimal: 32; HexadecimalCodec: 0x20</value>
    public static readonly StartOutcome C;

    /// <value>Decimal: 48; HexadecimalCodec: 0x30</value>
    public static readonly StartOutcome D;

    /// <value>Decimal: 240; HexadecimalCodec: 0xF0</value>
    public static readonly StartOutcome NotAvailable;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static StartOutcome()
    {
        const byte a = 0;
        const byte b = 16;
        const byte c = 32;
        const byte d = 48;
        const byte notAvailable = 240;

        A = new StartOutcome(a);
        B = new StartOutcome(b);
        C = new StartOutcome(c);
        D = new StartOutcome(d);
        NotAvailable = new StartOutcome(notAvailable);
        _ValueObjectMap = new Dictionary<byte, StartOutcome> {{a, A}, {b, B}, {c, C}, {d, D}, {notAvailable, NotAvailable}}
            .ToImmutableSortedDictionary();
    }

    private StartOutcome(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static StartOutcome Get(byte value)
    {
        if (!TryGet(value, out StartOutcome result))
        {
            throw new ArgumentOutOfRangeException(
                $"The argument {nameof(value)} with a value of {value} is not a valid value for {nameof(StartOutcome)}");
        }

        return result;
    }

    public static bool TryGet(byte value, out StartOutcome result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is StartOutcome outcomeParameterEntryPoint && Equals(outcomeParameterEntryPoint);
    public bool Equals(StartOutcome other) => _Value == other._Value;
    public bool Equals(StartOutcome x, StartOutcome y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 785879;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(StartOutcome left, StartOutcome right) => left._Value == right._Value;
    public static bool operator ==(StartOutcome left, byte right) => left._Value == right;
    public static bool operator ==(byte left, StartOutcome right) => left == right._Value;
    public static explicit operator byte(StartOutcome value) => value._Value;
    public static explicit operator short(StartOutcome value) => value._Value;
    public static explicit operator ushort(StartOutcome value) => value._Value;
    public static explicit operator int(StartOutcome value) => value._Value;
    public static explicit operator uint(StartOutcome value) => value._Value;
    public static explicit operator long(StartOutcome value) => value._Value;
    public static explicit operator ulong(StartOutcome value) => value._Value;
    public static bool operator !=(StartOutcome left, StartOutcome right) => !(left == right);
    public static bool operator !=(StartOutcome left, byte right) => !(left == right);
    public static bool operator !=(byte left, StartOutcome right) => !(left == right);

    #endregion
}