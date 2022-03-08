using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

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

    #region Instance Members

    /// <summary>
    ///     Gets the sequence of <see cref="TagLengthValue" /> objects in this object's Value field
    /// </summary>
    /// <returns></returns>
    public TagLengthValue[] DecodeValue() => _Codec.DecodeSiblings(_Value).AsTagLengthValues();

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ResponseMessageTemplateFormat1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ResponseMessageTemplateFormat1 Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<byte[]> result = _Codec.Decode(EncodingId, value) as DecodedResult<byte[]>
            ?? throw new DataElementParsingException(
                $"The {nameof(ResponseMessageTemplateFormat1)} could not be initialized because the {nameof(BerEncodingIdType.VariableCodec)} returned a null {nameof(DecodedResult<byte[]>)}");

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