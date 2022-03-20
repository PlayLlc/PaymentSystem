using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the Terminal data writing requests to be sent to the Card after processing the GENERATE AC command or th
///     RECOVER AC command.The value of this data object is composed of a series of TLVs. This data object may be provided
///     several times by the Terminal in a DET Signal.Therefore, these values must be accumulated in Tags To WriteYet After
///     Gen AC.
/// </summary>
public record TagsToWriteAfterGenAc : DataExchangeResponse, IEqualityComparer<TagsToWriteAfterGenAc>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8103;

    #endregion

    #region Constructor

    public TagsToWriteAfterGenAc(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    private static IEnumerable<PrimitiveValue> ResolveTagsToWrite(ReadOnlySpan<byte> value)
    {
        TagLengthValue[] tlv = _Codec.DecodeTagLengthValues(value).Where(a => _KnownPutDataTags.Any(b => b == a.GetTag())).ToArray();
        PrimitiveValue[] result = new PrimitiveValue[tlv.Length];

        for (int i = 0; i < tlv.Length; i++)
        {
            if (tlv[i].GetTag() == UnprotectedDataEnvelope1.Tag)
                result[i] = UnprotectedDataEnvelope1.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope2.Tag)
                result[i] = UnprotectedDataEnvelope2.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope3.Tag)
                result[i] = UnprotectedDataEnvelope3.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope4.Tag)
                result[i] = UnprotectedDataEnvelope4.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope5.Tag)
                result[i] = UnprotectedDataEnvelope5.Decode(tlv[i].EncodeValue(_Codec).AsSpan());
        }

        return result;
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TagsToWriteAfterGenAc Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TagsToWriteAfterGenAc Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TagsToWriteAfterGenAc Decode(ReadOnlySpan<byte> value) => new(ResolveTagsToWrite(value).ToArray());

    #endregion

    #region Equality

    public bool Equals(TagsToWriteAfterGenAc? x, TagsToWriteAfterGenAc? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToWriteAfterGenAc obj) => obj.GetHashCode();

    #endregion
}