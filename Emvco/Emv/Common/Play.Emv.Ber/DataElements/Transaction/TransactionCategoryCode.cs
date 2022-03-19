using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     This is a data object defined by MasterCard which indicates the type of transaction being performed, and which may
///     be used in card risk management.
/// </summary>
public record TransactionCategoryCode : PlayProprietaryDataElement<char[]>
{
    #region Static Metadata

    /// <value>Hex: 0x9F53 </value>
    public static readonly Tag Tag = 0x9F53;

    public static readonly PlayEncodingId EncodingId = AlphaNumericCodec.EncodingId;
    private const int _ByteLength = 1;

    #endregion

    #region Constructor

    public TransactionCategoryCode(char[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static TransactionCategoryCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override TransactionCategoryCode Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());
    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    public static TransactionCategoryCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        char[] result = PlayCodec.AlphaNumericCodec.DecodeToChars(value);

        return new TransactionCategoryCode(result);
    }

    #endregion
}