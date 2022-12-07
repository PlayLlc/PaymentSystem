using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     PCVC3(Track2) indicates to the Kernel the positions in the discretionary data field of the Track 2 Data where the
///     CVC3 (Track2) digits must be copied.
/// </summary>
public record PositionOfCardVerificationCode3Track2 : DataElement<ushort>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F65;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public PositionOfCardVerificationCode3Track2(ushort value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PositionOfCardVerificationCode3Track2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PositionOfCardVerificationCode3Track2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PositionOfCardVerificationCode3Track2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new PositionOfCardVerificationCode3Track2(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public int GetSetBitCount() => _Value.GetSetBitCount();

    internal Nibble[] GetBitFlagIndex()
    {
        Nibble[] result = new Nibble[_Value.GetSetBitCount()];
        ushort bufferValue = _Value;

        for (byte i = 0, j = 0; i < Specs.Integer.Int16.BitCount; i++)
        {
            if (bufferValue.IsBitSet(Bits.One))
                result[j++] = i;
        }

        return result;
    }

    #endregion
}