using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.DataElements.Exceptions;
using Play.Globalization;
using Play.Globalization.Currency;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the transaction amount above which the Kernel instantiates the CVM capabilities field in Terminal
///     Capabilities with CVM Capability – CVM Required.
/// </summary>
public record ReaderCvmRequiredLimit : DataElement<ulong>, IEqualityComparer<ReaderCvmRequiredLimit>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F74;
    public static readonly BerEncodingId BerEncodingId = NumericCodec.Identifier;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Constructor

    public ReaderCvmRequiredLimit(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Money AsMoney(CultureProfile cultureProfile)
    {
        return new(_Value, cultureProfile);
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    #endregion

    #region Serialization

    public static ReaderCvmRequiredLimit Decode(ReadOnlyMemory<byte> value)
    {
        return Decode(value.Span);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ReaderCvmRequiredLimit Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(BerEncodingId, value).ToUInt64Result()
            ?? throw new DataElementNullException(BerEncodingId);

        Check.Primitive.ForCharLength(result.CharCount, _CharLength, Tag);

        return new ReaderCvmRequiredLimit(result.Value);
    }

    public new byte[] EncodeValue()
    {
        return EncodeValue(_ByteLength);
    }

    #endregion

    #region Equality

    public bool Equals(ReaderCvmRequiredLimit? x, ReaderCvmRequiredLimit? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ReaderCvmRequiredLimit obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}