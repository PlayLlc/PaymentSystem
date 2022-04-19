using System.Collections.Immutable;
using System.Numerics;

using Play.Core;

namespace Play.Emv.Ber;

// TODO: This needs to be separated into an Enum and a struct value type. This enum is too large for a struct because of all the static metadata

/// <summary>
///     Identifies the status of the transaction (for example when the card can be removed) to be indicated through the
///     setting of lights or LEDs and/or the production of an audio signal. More information on Status is provided in
///     Book A section 9.2.
///     If the Status value is not recognized, the reader should ignore it and the current status of lights
///     or LEDS should not be changed and no audio signal should be produced as a result of the User Interface Request
/// </summary>
public record Statuses : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Statuses> _ValueObjectMap;

    /// <value>Decimal: 1; HexadecimalCodec: 0x1</value>
    public static readonly Statuses CardReadSuccessful;

    /// <value>Decimal: 2; HexadecimalCodec: 0x2</value>
    public static readonly Statuses Idle;

    /// <value>Decimal: 0; HexadecimalCodec: 0x0</value>
    public static readonly Statuses NotAvailable;

    /// <value>Decimal: 3; HexadecimalCodec: 0x3</value>
    public static readonly Statuses NotReady;

    /// <value>Decimal: 4; HexadecimalCodec: 0x4</value>
    public static readonly Statuses Processing;

    /// <value>Decimal: 5; HexadecimalCodec: 0x5</value>
    public static readonly Statuses ProcessingError;

    /// <value>Decimal: 6; HexadecimalCodec: 0x6</value>
    public static readonly Statuses ReadyToRead;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static Statuses()
    {
        const byte notAvailable = 0;
        const byte cardReadSuccessful = 1;
        const byte idle = 2;
        const byte notReady = 3;
        const byte processing = 4;
        const byte processingError = 5;
        const byte readyToRead = 6;

        NotAvailable = new Statuses(notAvailable);
        CardReadSuccessful = new Statuses(cardReadSuccessful);
        Idle = new Statuses(idle);
        NotReady = new Statuses(notReady);
        Processing = new Statuses(processing);
        ProcessingError = new Statuses(processingError);
        ReadyToRead = new Statuses(readyToRead);

        _ValueObjectMap = new Dictionary<byte, Statuses>
        {
            {cardReadSuccessful, CardReadSuccessful},
            {idle, Idle},
            {notAvailable, NotAvailable},
            {notReady, NotReady},
            {processing, Processing},
            {processingError, ProcessingError},
            {readyToRead, ReadyToRead}
        }.ToImmutableSortedDictionary();
    }

    internal Statuses(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static Statuses[] GetAll() => _ValueObjectMap.Values.ToArray();

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Statuses Get(byte value)
    {
        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(Statuses)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value];
    }

    #endregion

    #region Serialization

    public void Decode(Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = _Value;
    }

    #endregion

    #region Equality

    public bool Equals(Statuses x, Statuses y) => x.Equals(y);
    public override int GetHashCode() => 4440131 * _Value.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(Statuses left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Statuses right) => left == right._Value;
    public static explicit operator byte(Statuses value) => value._Value;
    public static explicit operator short(Statuses value) => value._Value;
    public static explicit operator ushort(Statuses value) => value._Value;
    public static explicit operator int(Statuses value) => value._Value;
    public static explicit operator uint(Statuses value) => value._Value;
    public static explicit operator long(Statuses value) => value._Value;
    public static explicit operator ulong(Statuses value) => value._Value;
    public static explicit operator BigInteger(Statuses value) => value._Value;
    public static bool operator !=(Statuses left, byte right) => !(left == right);
    public static bool operator !=(byte left, Statuses right) => !(left == right);

    #endregion
}