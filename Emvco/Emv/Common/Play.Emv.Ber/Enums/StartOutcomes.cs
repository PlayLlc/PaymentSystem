using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums;

public record StartOutcomes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly StartOutcomes Empty = new();
    private static readonly ImmutableSortedDictionary<byte, StartOutcomes> _ValueObjectMap;

    /// <value>Decimal: 0; HexadecimalCodec: 0x0</value>
    public static readonly StartOutcomes A;

    /// <value>Decimal: 16; HexadecimalCodec: 0x10</value>
    public static readonly StartOutcomes B;

    /// <value>Decimal: 32; HexadecimalCodec: 0x20</value>
    public static readonly StartOutcomes C;

    /// <value>Decimal: 48; HexadecimalCodec: 0x30</value>
    public static readonly StartOutcomes D;

    /// <value>Decimal: 240; HexadecimalCodec: 0xF0</value>
    public static readonly StartOutcomes NotAvailable;

    #endregion

    #region Constructor

    public StartOutcomes() : base()
    { }

    static StartOutcomes()
    {
        const byte a = 0;
        const byte b = 16;
        const byte c = 32;
        const byte d = 48;
        const byte notAvailable = 240;

        A = new StartOutcomes(a);
        B = new StartOutcomes(b);
        C = new StartOutcomes(c);
        D = new StartOutcomes(d);
        NotAvailable = new StartOutcomes(notAvailable);
        _ValueObjectMap = new Dictionary<byte, StartOutcomes> {{a, A}, {b, B}, {c, C}, {d, D}, {notAvailable, NotAvailable}}.ToImmutableSortedDictionary();
    }

    private StartOutcomes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override StartOutcomes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out StartOutcomes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static StartOutcomes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public static StartOutcomes Get(byte value)
    {
        if (!TryGet(value, out StartOutcomes result))
            throw new ArgumentOutOfRangeException($"The argument {nameof(value)} with a value of {value} is not a valid value for {nameof(StartOutcomes)}");

        return result;
    }

    public static bool TryGet(byte value, out StartOutcomes result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(StartOutcomes x, StartOutcomes y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 785879;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(StartOutcomes left, byte right) => left._Value == right;
    public static bool operator ==(byte left, StartOutcomes right) => left == right._Value;
    public static explicit operator byte(StartOutcomes value) => value._Value;
    public static explicit operator short(StartOutcomes value) => value._Value;
    public static explicit operator ushort(StartOutcomes value) => value._Value;
    public static explicit operator int(StartOutcomes value) => value._Value;
    public static explicit operator uint(StartOutcomes value) => value._Value;
    public static explicit operator long(StartOutcomes value) => value._Value;
    public static explicit operator ulong(StartOutcomes value) => value._Value;
    public static bool operator !=(StartOutcomes left, byte right) => !(left == right);
    public static bool operator !=(byte left, StartOutcomes right) => !(left == right);

    #endregion
}