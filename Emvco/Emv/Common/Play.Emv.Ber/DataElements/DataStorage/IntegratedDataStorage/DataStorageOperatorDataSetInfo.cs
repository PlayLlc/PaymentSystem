using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains Terminal provided data to be forwarded to the Card with the GENERATE AC command, as per DSDOL formatting.
/// </summary>
public record DataStorageOperatorDataSetInfo : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F54;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetInfo(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsPermanent() => _Value.IsBitSet(Bits.Eight);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsVolatile() => _Value.IsBitSet(Bits.Seven);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsLowVolatility() => _Value.IsBitSet(Bits.Six);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsDeclineOnDataStorageErrorSet() => _Value.IsBitSet(Bits.Four);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static DataStorageOperatorDataSetInfo Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static DataStorageOperatorDataSetInfo Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new DataStorageOperatorDataSetInfo(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion
}