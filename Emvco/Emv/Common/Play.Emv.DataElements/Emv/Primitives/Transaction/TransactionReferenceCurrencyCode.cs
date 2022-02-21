using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Code defining the common currency used by the terminal in case the Transaction Currency Code is different from the
///     Application Currency Code
/// </summary>
public record TransactionReferenceCurrencyCode : DataElement<ushort>, IEqualityComparer<TransactionReferenceCurrencyCode>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    public static readonly Tag Tag = 0x9F3C;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public TransactionReferenceCurrencyCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TransactionReferenceCurrencyCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TransactionReferenceCurrencyCode Decode(ReadOnlySpan<byte> value)
    {
        const ushort charLength = 3;

        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(BerEncodingId, value).ToUInt16Result()
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

        return new TransactionReferenceCurrencyCode(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Equality

    public bool Equals(TransactionReferenceCurrencyCode? x, TransactionReferenceCurrencyCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TransactionReferenceCurrencyCode obj) => obj.GetHashCode();

    #endregion
}