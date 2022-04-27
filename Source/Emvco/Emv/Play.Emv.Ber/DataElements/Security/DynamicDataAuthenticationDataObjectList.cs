using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     List of data objects (tag and length) to be passed to the ICC in the INTERNAL AUTHENTICATE command
/// </summary>
public record DynamicDataAuthenticationDataObjectList : DataObjectList, IEqualityComparer<DynamicDataAuthenticationDataObjectList>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F49;
    private const byte _MaxByteLength = 252;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="CardDataMissingException"></exception>
    public DynamicDataAuthenticationDataObjectList(params TagLength[] value) : base(value)
    {
        if (value.All(a => a.GetTag() != UnpredictableNumber.Tag))
            throw new CardDataMissingException($"The {nameof(DynamicDataAuthenticationDataObjectList)} must contain a tag for {nameof(UnpredictableNumber)}");
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DynamicDataAuthenticationDataObjectList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override DynamicDataAuthenticationDataObjectList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DynamicDataAuthenticationDataObjectList Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        return new DynamicDataAuthenticationDataObjectList(_Codec.DecodeTagLengths(value.ToArray()));
    }

    #endregion

    #region Equality

    public bool Equals(DynamicDataAuthenticationDataObjectList? x, DynamicDataAuthenticationDataObjectList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DynamicDataAuthenticationDataObjectList obj) => obj.GetHashCode();

    #endregion
}