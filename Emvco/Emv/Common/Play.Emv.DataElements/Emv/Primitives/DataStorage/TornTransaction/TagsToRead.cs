using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Description: List of tags indicating the data the Terminal has requested to be read. This data item is present if
///     the Terminal wants any data back from the Card before the Data Record. This could be in the context of SDS, or for
///     non data storage usage reasons, for example the PAN. This data item may contain configured data. This data object
///     may be provided several times by the Terminal. Therefore, the values of each of these tags must be accumulated in
///     the Tags To Read Yet buffer. A.1.154 Tags To Read Yet
/// </summary>
public record TagsToRead : DataExchangeRequest, IEqualityComparer<TagsToRead>
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

    /// TODO: Exqueese me? Making this guy shut up for now. Book C-2 section 3.6.2 says "The process continues until
    /// TODO: all records have been read" so unless there's some optimization reason that i find out about later
    /// TODO: there's not really a use for this

    //public override Tag GetNextAvailableTagFromCard() => throw new NotImplementedException();

    #endregion

    #region Serialization

    public static TagsToRead Decode(ReadOnlyMemory<byte> value)
    {
        if (value.IsEmpty)
            return new TagsToRead();

        return new TagsToRead(_Codec.DecodeTagSequence(value.Span));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TagsToRead Decode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return new TagsToRead();

        // This Value field is already BER encoded, which is what this object's _Value field requires
        return new TagsToRead(_Codec.DecodeTagSequence(value));
    }

    #endregion
}