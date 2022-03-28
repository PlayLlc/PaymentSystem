using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: List of tags indicating the data the Terminal has requested to be read. This data item is present if
///     the Terminal wants any data back from the Card before the Data Record. This could be in the context of SDS, or for
///     non data storage usage reasons, for example the PAN. This data item may contain configured data. This data object
///     may be provided several times by the Terminal. Therefore, the values of each of these tags must be accumulated in
///     the Tags To Read Yet buffer. A.1.154 Tags To Read Yet
/// </summary>
public record TagsToRead : DataExchangeRequest, IEqualityComparer<PrimitiveValue>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BerEncodingIdType.VariableCodec;
    public static readonly Tag Tag = 0xDF8112;

    #endregion

    #region Constructor

    public TagsToRead(Tag[] tags) : base(tags)
    { }

    public TagsToRead() : base(Array.Empty<Tag>())
    { }

    #endregion

    #region Serialization

    public override TagsToRead Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static TagsToRead Decode(ReadOnlyMemory<byte> value)
    {
        if (value.IsEmpty)
            return new TagsToRead();

        return new TagsToRead(_Codec.DecodeTagSequence(value.Span));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static TagsToRead Decode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return new TagsToRead();

        // This Value field is already BER encoded, which is what this object's _Value field requires
        return new TagsToRead(_Codec.DecodeTagSequence(value));
    }

    #endregion

    #region Equality

    public bool Equals(TagsToRead? x, TagsToRead? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToRead obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public Tag[] AsTags() => _Value.ToArray();

    /// <summary>
    ///     Searches the <see cref="IReadTlvDatabase" /> for any <see cref="Tag" /> values requested by this
    ///     <see cref="TagsToRead" />. If any of the requested values are present in the <see cref="IReadTlvDatabase" />, those
    ///     <see cref="Tag" /> values are removed from this list. The method returns the <see cref="PrimitiveValue" /> objects
    ///     that were successfully resolved by the <see cref="IReadTlvDatabase" />
    /// </summary>
    /// <returns>
    ///     A list of the <see cref="PrimitiveValue" /> objects that were successfully resolved by the
    ///     <see cref="IReadTlvDatabase" />
    /// </returns>
    /// <warning>
    ///     This should be the only method available to resolve the requested tags. That way we're consistent in how we're
    ///     handling data exchange
    /// </warning>
    /// <exception cref="TerminalDataException"></exception>
    public IEnumerable<PrimitiveValue> Resolve(IReadTlvDatabase database)
    {
        for (nint i = 0; i < _Value.Count; i++)
        {
            if (!TryDequeue(out Tag tag))
                throw new TerminalDataException($"The {nameof(TagsToRead)} could not dequeue a value from memory");

            if (!database.IsPresentAndNotEmpty(tag))
                Enqueue(tag);

            yield return database.Get(tag);
        }
    }

    public Tag[] GetTagsToReadYet() => _Value.ToArray();

    #endregion
}