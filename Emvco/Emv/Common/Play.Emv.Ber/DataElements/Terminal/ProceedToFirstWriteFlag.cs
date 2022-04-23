using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates that the Terminal will send no more requests to read data other than as indicated in Tags To Read. This
///     data item indicates the point at which the Kernel shifts from the Card reading phase to the Card writing phase. If
///     Proceed To First Write Flag is not present or is present with non zero length and value different from zero, then
///     the Kernel proceeds without waiting. If Proceed To First Write Flag is present with zero length, then the Kernel
///     sends a DEK Signal to the Terminal and waits for the DET Signal. If Proceed To First Write Flag is present with non
///     zero length and value equal to zero, then the Kernel waits for a DET Signal from the Terminal without sending a DEK
///     Signal.
/// </summary>
public record ProceedToFirstWriteFlag : DataElement<byte>, IEqualityComparer<ProceedToFirstWriteFlag>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8110;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public ProceedToFirstWriteFlag(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProceedToFirstWriteFlag Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override ProceedToFirstWriteFlag Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ProceedToFirstWriteFlag Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.BinaryCodec.DecodeToByte(value);

        return new ProceedToFirstWriteFlag(result);
    }

    public new byte[] EncodeValue() => Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(ProceedToFirstWriteFlag? x, ProceedToFirstWriteFlag? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ProceedToFirstWriteFlag obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(ProceedToFirstWriteFlag value) => value._Value;

    #endregion
}