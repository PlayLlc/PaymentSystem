using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the time that the field is to be turned off after the transaction is completed if requested to do so by
///     the  cardholder device
/// </summary>
public record HoldTimeValue : DataElement<Milliseconds>, IEqualityComparer<HoldTimeValue>
{
    #region Static Metadata

    private static readonly Milliseconds _MinimumValue = new(100);
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8130;
    private const byte _ByteLength = 3;

    #endregion

    #region Constructor

    /// <param name="value">
    ///     Minimum Value: 100 ms
    /// </param>
    public HoldTimeValue(Milliseconds value) : base(value)
    {
        if (value < _MinimumValue)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} must be at least 100 ms to initialize a {nameof(HoldTimeValue)}");
        }
    }

    #endregion

    #region Instance Members

    public Milliseconds AsMilliseconds() => _Value;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <summary>
    ///     The hold time in units of 100 ms
    /// </summary>
    public ulong GetHoldTime() => (ulong) _Value / 100;

    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static HoldTimeValue Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    private static HoldTimeValue Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 6;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new DataElementNullException(EncodingId);

        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

        return new HoldTimeValue(new Milliseconds(result.Value * 100));
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(GetEncodingId(), _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(HoldTimeValue? x, HoldTimeValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(HoldTimeValue obj) => obj.GetHashCode();

    #endregion
}