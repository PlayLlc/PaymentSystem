using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the transaction amount above which the transaction is not allowed, when on device cardholder verification
///     is not supported.
/// </summary>
public record ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice : ReaderContactlessTransactionLimit
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8124;

    #endregion

    #region Constructor

    public ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount() => _ByteLength;

    #endregion

    #region Serialization

    public static ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        return new ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice(result);
    }

    #endregion
}