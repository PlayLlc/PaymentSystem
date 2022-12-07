using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Enums.Interchange;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public record PosEntryMode : DataElement<byte>, IEqualityComparer<PosEntryMode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F39;
    private const byte _ByteLength = 1;
    private const byte _DigitsLength = 2;

    #endregion

    #region Constructor

    public PosEntryMode(byte value) : base(value)
    { }

    public PosEntryMode(PosEntryModes value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static PosEntryMode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PosEntryMode Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static PosEntryMode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.NumericCodec.DecodeToByte(value);

        Check.Primitive.ForCharLength(result.GetNumberOfDigits(), _DigitsLength, Tag);

        return new PosEntryMode(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(PosEntryMode? x, PosEntryMode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PosEntryMode obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(PosEntryMode value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => PlayCodec.NumericCodec.GetByteCount(_Value);
    public override ushort GetValueByteCount() => PlayCodec.NumericCodec.GetByteCount(_Value);

    #endregion
}