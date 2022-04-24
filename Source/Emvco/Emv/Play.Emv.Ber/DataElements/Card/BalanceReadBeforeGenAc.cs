﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     The presence of Balance Read Before Gen AC in the TLV Database is an indication to the Kernel to read the offline
///     balance from the Card before the GENERATE AC command. The Kernel stores the offline balance read from the Card in
///     Balance Read Before Gen AC.
/// </summary>
public record BalanceReadBeforeGenAc : DataElement<ulong>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8104;
    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    private const byte _ByteLength = 6;
    private const byte _CharLength = 12;

    #endregion

    #region Constructor

    public BalanceReadBeforeGenAc(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="CodecParsingException"></exception>
    public static BalanceReadBeforeGenAc Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override BalanceReadBeforeGenAc Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="CodecParsingException"></exception>
    public static BalanceReadBeforeGenAc Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        ulong result = PlayCodec.NumericCodec.DecodeToUInt64(value);

        Check.Primitive.ForMaxCharLength(result.GetNumberOfDigits(), _CharLength, Tag);

        return new BalanceReadBeforeGenAc(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion
}