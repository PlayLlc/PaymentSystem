using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the data objects (without tags and lengths) returned by the ICC in response to a command
/// </summary>
public record ResponseMessageTemplateFormat1 : DataElement<byte[]>, IEqualityComparer<ResponseMessageTemplateFormat1>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = VariableCodec.Identifier;
    public static readonly Tag Tag = 0x80;

    #endregion

    #region Constructor

    public ResponseMessageTemplateFormat1(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Gets the sequence of <see cref="TagLengthValue" /> objects in this object's Value field
    /// </summary>
    /// <returns></returns>
    public TagLengthValue[] DecodeValue() => _Codec.DecodeSiblings(_Value).AsTagLengthValues();

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ResponseMessageTemplateFormat1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ResponseMessageTemplateFormat1 Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<byte[]> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<byte[]>
            ?? throw new InvalidOperationException(
                $"The {nameof(ResponseMessageTemplateFormat1)} could not be initialized because the {nameof(VariableCodec)} returned a null {nameof(DecodedResult<byte[]>)}");

        return new ResponseMessageTemplateFormat1(result.Value);
    }

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
}