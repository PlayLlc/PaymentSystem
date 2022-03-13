using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the currency in which the account is managed in accordance with [ISO 4217].
/// </summary>
public record ApplicationCurrencyCode : DataElement<ushort>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F42;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public ApplicationCurrencyCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationCurrencyCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static ApplicationCurrencyCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.NumericCodec.DecodeToUInt16(value);

        return new ApplicationCurrencyCode(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion
}