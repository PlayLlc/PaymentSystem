using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     List of data objects indicating the Terminal data writing  requests to be sent to the Card before processing the
///     GENERATE AC command or the RECOVER AC command. This data object may be provided several times by the  Terminal in a
///     DET Signal. Therefore, these values must be  accumulated in Tags To Write Yet Before Gen AC buffer.
/// </summary>
public record TagsToWriteBeforeGeneratingApplicationCryptogram : DataExchangeResponse, IEqualityComparer<TagsToWriteBeforeGeneratingApplicationCryptogram>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8102;

    #endregion

    #region Constructor

    public TagsToWriteBeforeGeneratingApplicationCryptogram(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TagsToWriteAfterGeneratingApplicationCryptogram Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TagsToWriteAfterGeneratingApplicationCryptogram Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TagsToWriteAfterGeneratingApplicationCryptogram Decode(ReadOnlySpan<byte> value) => new(ResolveTagsToWrite(value).ToArray());

    #endregion

    #region Equality

    public bool Equals(TagsToWriteBeforeGeneratingApplicationCryptogram? x, TagsToWriteBeforeGeneratingApplicationCryptogram? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToWriteBeforeGeneratingApplicationCryptogram obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
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
}