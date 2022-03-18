using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Value to provide variability and uniqueness to the generation of a cryptogram
/// </summary>
public record UnpredictableNumber : DataElement<uint>, IEqualityComparer<UnpredictableNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F37;
    private const byte _ByteLength = 4;

    #endregion

    #region Constructor

    public UnpredictableNumber(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ushort GetByteCount() => _ByteLength;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static UnpredictableNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static UnpredictableNumber Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new UnpredictableNumber(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(UnpredictableNumber? x, UnpredictableNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(UnpredictableNumber obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(UnpredictableNumber value) => value._Value;

    #endregion
}