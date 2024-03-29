using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the data objects (without tags and lengths) returned by the ICC in response to a command
/// </summary>
public record ResponseMessageTemplateFormat1 : DataElement<byte[]>, IEqualityComparer<ResponseMessageTemplateFormat1>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BerEncodingIdType.VariableCodec;
    public static readonly Tag Tag = 0x80;

    #endregion

    #region Constructor

    public ResponseMessageTemplateFormat1(byte[] value) : base(value)
    { }

    #endregion

    #region Serialization

    public static ResponseMessageTemplateFormat1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override ResponseMessageTemplateFormat1 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public static ResponseMessageTemplateFormat1 Decode(ReadOnlySpan<byte> value) => new ResponseMessageTemplateFormat1(value.ToArray());

    #endregion

    #region Equality

    public bool Equals(ResponseMessageTemplateFormat1? x, ResponseMessageTemplateFormat1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ResponseMessageTemplateFormat1 obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    /// <summary>
    ///     Gets the sequence of <see cref="TagLengthValue" /> objects in this object's Value field
    /// </summary>
    /// <returns></returns>
    public TagLengthValue[] DecodeValue() => _Codec.DecodeSiblings(_Value).AsTagLengthValues();

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}