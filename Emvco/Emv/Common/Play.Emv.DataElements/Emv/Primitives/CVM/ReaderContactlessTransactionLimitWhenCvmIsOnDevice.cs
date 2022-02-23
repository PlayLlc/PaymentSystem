using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the transaction amount above which the transaction is not allowed, when on device cardholder verification
///     is supported.
/// </summary>
public record ReaderContactlessTransactionLimitWhenCvmIsOnDevice : ReaderContactlessTransactionLimit
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8125;

    #endregion

    #region Constructor

    public ReaderContactlessTransactionLimitWhenCvmIsOnDevice(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ReaderContactlessTransactionLimitWhenCvmIsOnDevice Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ReaderContactlessTransactionLimitWhenCvmIsOnDevice Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result()
            ?? throw new DataElementNullException(PlayEncodingId);

        return new ReaderContactlessTransactionLimitWhenCvmIsOnDevice(result.Value);
    }

    #endregion
}