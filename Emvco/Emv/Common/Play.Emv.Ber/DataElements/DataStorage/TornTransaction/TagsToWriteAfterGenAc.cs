using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Emv.Kernel2.Databases;

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

    private static readonly Tag[] _KnownTags = new Tag[]
    {
        UnprotectedDataEnvelope1.Tag, UnprotectedDataEnvelope2.Tag, UnprotectedDataEnvelope3.Tag, UnprotectedDataEnvelope4.Tag,
        UnprotectedDataEnvelope5.Tag
    };

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
    private static IEnumerable<PrimitiveValue> ResolveTagsToWrite(ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length;)
        {
            TagLengthValue currentTagLengthValue = _Codec.DecodeTagLengthValue(value[..i]);
            i += checked((int) currentTagLengthValue.GetTagLengthValueByteCount());

            if (UnprotectedDataEnvelope1.TryDecoding(currentTagLengthValue, out UnprotectedDataEnvelope1? unprotectedDataEnvelope1))
                yield return unprotectedDataEnvelope1!;
            if (UnprotectedDataEnvelope2.TryDecoding(currentTagLengthValue, out UnprotectedDataEnvelope2? unprotectedDataEnvelope2))
                yield return unprotectedDataEnvelope2!;
            if (UnprotectedDataEnvelope3.TryDecoding(currentTagLengthValue, out UnprotectedDataEnvelope3? unprotectedDataEnvelope3))
                yield return unprotectedDataEnvelope3!;
            if (UnprotectedDataEnvelope4.TryDecoding(currentTagLengthValue, out UnprotectedDataEnvelope4? unprotectedDataEnvelope4))
                yield return unprotectedDataEnvelope4!;
            if (UnprotectedDataEnvelope5.TryDecoding(currentTagLengthValue, out UnprotectedDataEnvelope5? unprotectedDataEnvelope5))
                yield return unprotectedDataEnvelope5!;
        }
    }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static TagsToWriteAfterGenAc Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PrimitiveValue Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

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