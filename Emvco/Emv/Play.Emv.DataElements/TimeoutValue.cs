using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;
using Play.Globalization.Time;

namespace Play.Emv.DataElements;

public record TimeoutValue : DataElement<ushort>, IEqualityComparer<TimeoutValue>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryIntegerCodec.Identifier;
    public static readonly Tag Tag = 0xDF8127;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TimeoutValue(ushort value) : base(value)
    { }

    public TimeoutValue(Milliseconds value) : base((ushort) value)
    {
        if (value > ushort.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value));
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TimeoutValue Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TimeoutValue Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = codec.Decode(BerEncodingId, value) as DecodedResult<ushort>
            ?? throw new DataElementNullException(BerEncodingId);

        return new TimeoutValue(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(BerEncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(TimeoutValue? x, TimeoutValue? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TimeoutValue obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator Milliseconds(TimeoutValue value) => new(value._Value);

    #endregion
}