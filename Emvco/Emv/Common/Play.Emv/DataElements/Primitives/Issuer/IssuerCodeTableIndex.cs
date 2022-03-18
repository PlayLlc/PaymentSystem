using System;
using System.Collections.Generic;

using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the code table according to ISO/IEC 8859 for displaying the Application Preferred Name
/// </summary>
public record IssuerCodeTableIndex : DataElement<byte>, IEqualityComparer<IssuerCodeTableIndex>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F11;
    private const byte _ByteLength = 1;
    private const byte _CharLength = 2;

    #endregion

    #region Constructor

    public IssuerCodeTableIndex(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public static bool StaticEquals(IssuerCodeTableIndex? x, IssuerCodeTableIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static AuthorizationResponseCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static AuthorizationResponseCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.NumericCodec.DecodeToByte(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new AuthorizationResponseCode(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion

    #region Equality

    public bool Equals(IssuerCodeTableIndex? x, IssuerCodeTableIndex? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerCodeTableIndex obj) => obj.GetHashCode();

    #endregion
}