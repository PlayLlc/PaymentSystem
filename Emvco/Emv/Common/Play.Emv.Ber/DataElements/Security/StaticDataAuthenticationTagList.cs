using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Description: List of tags of primitive data objects defined in this specification for which the value fields must
///     be included in the static data to be signed.
/// </summary>
public record StaticDataAuthenticationTagList : DataElement<Tag[]>, IEqualityComparer<StaticDataAuthenticationTagList>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F4A;
    private const byte _MaxByteLength = 250;

    #endregion

    #region Constructor

    public StaticDataAuthenticationTagList(Tag[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public Tag[] GetRequiredTags() => _Value;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static StaticDataAuthenticationTagList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override StaticDataAuthenticationTagList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static StaticDataAuthenticationTagList Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        return new StaticDataAuthenticationTagList(Codec.DecodeTagSequence(value));
    }

    #endregion

    #region Equality

    public bool Equals(StaticDataAuthenticationTagList? x, StaticDataAuthenticationTagList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(StaticDataAuthenticationTagList obj) => obj.GetHashCode();

    #endregion
}