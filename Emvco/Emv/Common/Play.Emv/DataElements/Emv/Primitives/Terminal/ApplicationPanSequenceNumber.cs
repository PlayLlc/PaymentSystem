using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record ApplicationPanSequenceNumber : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5F34;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public ApplicationPanSequenceNumber(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static ApplicationPanSequenceNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static ApplicationPanSequenceNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataElementParsingException(EncodingId);

        return new ApplicationPanSequenceNumber(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(ApplicationPanSequenceNumber? x, ApplicationPanSequenceNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationPanSequenceNumber obj) => obj.GetHashCode();

    #endregion
}