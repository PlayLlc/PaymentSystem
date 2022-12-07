using System.Collections.Immutable;
using System.Numerics;

using Play.Core;

namespace Play.Emv.Ber.Enums;

// TODO: This needs to be separated into an Enum and a struct value type. This enum is too large for a struct because of all the static metadata

/// <summary>
///     Identifies the status of the transaction (for example when the card can be removed) to be indicated through the
///     setting of lights or LEDs and/or the production of an audio signal. More information on Status is provided in Book
///     A section 9.2. If the Status value is not recognized, the reader should ignore it and the current status of lights
///     or LEDS should not be changed and no audio signal should be produced as a result of the User Interface Request
/// </summary>
public record DisplayStatuses : EnumObject<byte>
{
    #region Static Metadata

    public static readonly DisplayStatuses Empty = new();
    private static readonly ImmutableSortedDictionary<byte, DisplayStatuses> _ValueObjectMap;

    /// <value>Decimal: 1; HexadecimalCodec: 0x1</value>
    public static readonly DisplayStatuses CardReadSuccessful;

    /// <value>Decimal: 2; HexadecimalCodec: 0x2</value>
    public static readonly DisplayStatuses Idle;

    /// <value>Decimal: 0; HexadecimalCodec: 0x0</value>
    public static readonly DisplayStatuses NotAvailable;

    /// <value>Decimal: 3; HexadecimalCodec: 0x3</value>
    public static readonly DisplayStatuses NotReady;

    /// <value>Decimal: 4; HexadecimalCodec: 0x4</value>
    public static readonly DisplayStatuses Processing;

    /// <value>Decimal: 5; HexadecimalCodec: 0x5</value>
    public static readonly DisplayStatuses ProcessingError;

    /// <value>Decimal: 6; HexadecimalCodec: 0x6</value>
    public static readonly DisplayStatuses ReadyToRead;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public DisplayStatuses()
    { }

    static DisplayStatuses()
    {
        const byte notAvailable = 0;
        const byte cardReadSuccessful = 1;
        const byte idle = 2;
        const byte notReady = 3;
        const byte processing = 4;
        const byte processingError = 5;
        const byte readyToRead = 6;

        NotAvailable = new DisplayStatuses(notAvailable);
        CardReadSuccessful = new DisplayStatuses(cardReadSuccessful);
        Idle = new DisplayStatuses(idle);
        NotReady = new DisplayStatuses(notReady);
        Processing = new DisplayStatuses(processing);
        ProcessingError = new DisplayStatuses(processingError);
        ReadyToRead = new DisplayStatuses(readyToRead);

        _ValueObjectMap = new Dictionary<byte, DisplayStatuses>
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

    internal DisplayStatuses(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override DisplayStatuses[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out DisplayStatuses? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static DisplayStatuses Get(byte value)
    {
        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(DisplayStatuses)} could be retrieved because the argument provided does not match a definition value");
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

    public bool Equals(DisplayStatuses x, DisplayStatuses y) => x.Equals(y);
    public override int GetHashCode() => 4440131 * _Value.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(DisplayStatuses left, byte right) => left._Value == right;
    public static bool operator ==(byte left, DisplayStatuses right) => left == right._Value;
    public static explicit operator byte(DisplayStatuses value) => value._Value;
    public static explicit operator short(DisplayStatuses value) => value._Value;
    public static explicit operator ushort(DisplayStatuses value) => value._Value;
    public static explicit operator int(DisplayStatuses value) => value._Value;
    public static explicit operator uint(DisplayStatuses value) => value._Value;
    public static explicit operator long(DisplayStatuses value) => value._Value;
    public static explicit operator ulong(DisplayStatuses value) => value._Value;
    public static explicit operator BigInteger(DisplayStatuses value) => value._Value;
    public static bool operator !=(DisplayStatuses left, byte right) => !(left == right);
    public static bool operator !=(byte left, DisplayStatuses right) => !(left == right);

    #endregion
}