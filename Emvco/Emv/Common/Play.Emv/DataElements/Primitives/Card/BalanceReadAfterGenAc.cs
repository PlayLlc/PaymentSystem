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
///     The presence of Balance Read After Gen AC in the TLV Database is an indication to the Kernel to read the offline
///     balance from the Card after the GENERATE AC command. The Kernel stores the offline balance read from the Card in
///     Balance Read After Gen AC.
/// </summary>
public record BalanceReadAfterGenAc : DataElement<ulong>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8105;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Constructor

    public BalanceReadAfterGenAc(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static BalanceReadAfterGenAc Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static BalanceReadAfterGenAc Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new BalanceReadAfterGenAc(result);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);
    public new byte[] EncodeValue(int length) => EncodeValue();

    #endregion
}