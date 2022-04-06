using System.Collections.Immutable;
using System.Numerics;

namespace Play.Emv.Ber;

// TODO: This needs to be separated into an Enum and a struct value type. This enum is too large for a struct because of all the static metadata

/// <summary>
///     Identifies the status of the transaction (for example when the card can be removed) to be indicated through the
///     setting of lights or LEDs and/or the production of an audio signal. More information on Status is provided in
///     Book A section 9.2.
///     If the Status value is not recognised, the reader should ignore it and the current status of lights
///     or LEDS should not be changed and no audio signal should be produced as a result of the User Interface Request
/// </summary>
public readonly struct Status
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Status> _ValueObjectMap;

    /// <value>Decimal: 1; HexadecimalCodec: 0x1</value>
    public static readonly Status CardReadSuccessful;

    /// <value>Decimal: 2; HexadecimalCodec: 0x2</value>
    public static readonly Status Idle;

    /// <value>Decimal: 0; HexadecimalCodec: 0x0</value>
    public static readonly Status NotAvailable;

    /// <value>Decimal: 3; HexadecimalCodec: 0x3</value>
    public static readonly Status NotReady;

    /// <value>Decimal: 4; HexadecimalCodec: 0x4</value>
    public static readonly Status Processing;

    /// <value>Decimal: 5; HexadecimalCodec: 0x5</value>
    public static readonly Status ProcessingError;

    /// <value>Decimal: 6; HexadecimalCodec: 0x6</value>
    public static readonly Status ReadyToRead;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static Status()
    {
        const byte notAvailable = 0;
        const byte cardReadSuccessful = 1;
        const byte idle = 2;
        const byte notReady = 3;
        const byte processing = 4;
        const byte processingError = 5;
        const byte readyToRead = 6;

        NotAvailable = new Status(notAvailable);
        CardReadSuccessful = new Status(cardReadSuccessful);
        Idle = new Status(idle);
        NotReady = new Status(notReady);
        Processing = new Status(processing);
        ProcessingError = new Status(processingError);
        ReadyToRead = new Status(readyToRead);

        _ValueObjectMap = new Dictionary<byte, Status>
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

    internal Status(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static Status Get(byte value)
    {
        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(Status)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value];
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is Status Status && Equals(Status);
    public bool Equals(Status other) => _Value == other._Value;
    public bool Equals(Status x, Status y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;
    public override int GetHashCode() => 4440131 * _Value.GetHashCode();

    #endregion

    #region Serialization

    public void Decode(Span<byte> buffer, ref int offset)
    {
        buffer[offset++] = _Value;
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Status left, Status right) => left._Value == right._Value;
    public static bool operator ==(Status left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Status right) => left == right._Value;
    public static explicit operator byte(Status value) => value._Value;
    public static explicit operator short(Status value) => value._Value;
    public static explicit operator ushort(Status value) => value._Value;
    public static explicit operator int(Status value) => value._Value;
    public static explicit operator uint(Status value) => value._Value;
    public static explicit operator long(Status value) => value._Value;
    public static explicit operator ulong(Status value) => value._Value;
    public static explicit operator BigInteger(Status value) => value._Value;
    public static bool operator !=(Status left, Status right) => !(left == right);
    public static bool operator !=(Status left, byte right) => !(left == right);
    public static bool operator !=(byte left, Status right) => !(left == right);

    #endregion
}