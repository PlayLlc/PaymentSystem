﻿using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Data Storage EncodingId constructed as follows: Application PAN (without any 'F' padding) || Application PAN
///     Sequence Number If necessary, it is padded to the left with one hexadecimal zero  to ensure whole bytes. If
///     necessary, it is padded to the left with hexadecimal zeroes to  ensure a minimum length of 8 bytes.
/// </summary>
public record DataStorageId : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F5E;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 11;

    #endregion

    #region Constructor

    public DataStorageId(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageId Decode(ReadOnlySpan<byte> value)
    {
        const byte minCharLength = 16;
        const byte maxCharLength = 22;
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(PlayEncodingId, value).ToBigInteger()
            ?? throw new DataElementNullException(PlayEncodingId);

        Check.Primitive.ForMinCharLength(result.CharCount, minCharLength, Tag);
        Check.Primitive.ForMaxCharLength(result.CharCount, maxCharLength, Tag);

        return new DataStorageId(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataStorageId Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}