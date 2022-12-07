using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: Represents the amount of offline spending available in the Card. The OfflineOnly Accumulator Balance
///     is  retrievable by the GET DATA command, if allowed by the Card configuration.
/// </summary>
public record OfflineAccumulatorBalance : DataElement<ulong>, IEqualityComparer<OfflineAccumulatorBalance>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F50;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Constructor

    public OfflineAccumulatorBalance(ulong value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static OfflineAccumulatorBalance Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override OfflineAccumulatorBalance Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static OfflineAccumulatorBalance Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        Check.Primitive.ForCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new OfflineAccumulatorBalance(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(OfflineAccumulatorBalance? x, OfflineAccumulatorBalance? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(OfflineAccumulatorBalance obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => _ByteLength;

    public override ushort GetValueByteCount() => PlayCodec.NumericCodec.GetByteCount(_Value);

    #endregion
}