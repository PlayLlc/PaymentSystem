using System;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     The presence of Balance Read After Gen AC in the TLV Database is an indication to the Kernel to read the offline
///     balance from the Card after the GENERATE AC command. The Kernel stores the offline balance read from the Card in
///     Balance Read After Gen AC.
/// </summary>
public record BalanceReadAfterGenAc : DataElement<ushort>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8105;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 6;

    #endregion

    #region Constructor

    public BalanceReadAfterGenAc(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static BalanceReadAfterGenAc Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static BalanceReadAfterGenAc Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value) as DecodedResult<ushort>
            ?? throw new DataElementParsingException(
                $"The {nameof(BalanceReadAfterGenAc)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        Check.Primitive.ForMaxCharLength(result.CharCount, 12, Tag);

        return new BalanceReadAfterGenAc(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public override byte[] EncodeValue(BerCodec codec) => EncodeValue();

    #endregion
}