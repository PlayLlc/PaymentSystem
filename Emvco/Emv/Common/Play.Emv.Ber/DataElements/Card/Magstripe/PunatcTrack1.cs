using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core;
using Play.Core.Extensions;
using Play.Core.Specifications;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     PUNATC(Track1) indicates to the Kernel the positions in the discretionary data field of Track 1 Data where the
///     Unpredictable Number (NumericCodec) digits and Application Transaction Counter digits have to be copied.
/// </summary>
public record PunatcTrack1 : DataElement<ulong>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F63;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public PunatcTrack1(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public int GetSetBitCount() => _Value.GetSetBitCount();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    internal Nibble[] GetBitFlagIndex()
    {
        Nibble[] result = new Nibble[_Value.GetSetBitCount()];
        ulong bufferValue = _Value;

        for (byte i = 0, j = 0; i < Specs.Integer.Int64.BitCount; i++)
        {
            if (bufferValue.IsBitSet(Bits.One))
                result[j++] = i;
        }

        return result;
    }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PunatcTrack1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PunatcTrack1 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static PunatcTrack1 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.BinaryCodec.DecodeToUInt64(value);

        return new PunatcTrack1(result);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion
}