using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

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

    #endregion

    #region Serialization

    public static ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value).ToUInt64Result()
            ?? throw new DataElementNullException(BerEncodingId);

        return new ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice(result.Value);
    }

    #endregion
}