using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
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

    public static ApplicationCurrencyCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static ApplicationCurrencyCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value) as DecodedResult<ushort>
            ?? throw new DataElementParsingException(
                $"The {nameof(ApplicationCurrencyCode)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        Check.Primitive.ForMaxCharLength(result.CharCount, 3, Tag);

        return new ApplicationCurrencyCode(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec codec) => EncodeValue();

    #endregion
}