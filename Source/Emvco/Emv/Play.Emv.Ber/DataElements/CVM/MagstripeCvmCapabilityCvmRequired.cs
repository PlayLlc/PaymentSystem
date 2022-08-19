using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the CVM capability of the Terminal/Reader in the case of a mag-stripe mode transaction when the Amount,
///     Authorized (Numeric) is greater than the Reader CVM Required Limit.
/// </summary>
public record MagstripeCvmCapabilityCvmRequired : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811E;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public MagstripeCvmCapabilityCvmRequired(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MagstripeCvmCapabilityCvmRequired Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override MagstripeCvmCapabilityCvmRequired Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static MagstripeCvmCapabilityCvmRequired Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new MagstripeCvmCapabilityCvmRequired(result);
    }

    public override byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(int length) => _Codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Operator Overrides

    public static implicit operator byte(MagstripeCvmCapabilityCvmRequired value) => value._Value;

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}