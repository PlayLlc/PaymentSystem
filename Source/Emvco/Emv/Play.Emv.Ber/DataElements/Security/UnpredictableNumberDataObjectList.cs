using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The UDOL is the DOL that specifies the data objects to be included in the data field of the COMPUTE CRYPTOGRAPHIC
///     CHECKSUM command. The UDOL must at least include the Unpredictable Number (Numeric). The UDOL is not mandatory for
///     the Card. If it is not present in the Card, then the Default UDOL is used.
/// </summary>
public record UnpredictableNumberDataObjectList : DataObjectList
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F69;
    private static readonly byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public UnpredictableNumberDataObjectList(params TagLength[] value) : base(value)
    {
        Check.Primitive.ForMaximumLength(value.Sum(a => a.GetTagLengthByteCount()), _MaxByteLength, Tag);
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    public static UnpredictableNumberDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override UnpredictableNumberDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static UnpredictableNumberDataObjectList Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodeTagLengths(value.ToArray()));

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}