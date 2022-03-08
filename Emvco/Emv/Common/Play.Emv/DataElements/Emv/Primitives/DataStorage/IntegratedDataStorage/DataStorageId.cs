using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.DataStorage.IntegratedDataStorage;

/// <summary>
///     Data Storage EncodingId constructed as follows: Application PAN (without any 'F' padding) || Application PAN
///     Sequence Number If necessary, it is padded to the left with one hexadecimal zero  to ensure whole bytes. If
///     necessary, it is padded to the left with hexadecimal zeroes to  ensure a minimum length of 8 bytes.
/// </summary>
public record DataStorageId : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F5E;
    private const byte _MinByteLength = 8;
    private const byte _MaxByteLength = 11;

    #endregion

    #region Constructor

    public DataStorageId(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="DataElementNullException"></exception>
    public static DataStorageId Decode(ReadOnlySpan<byte> value)
    {
        const byte minCharLength = 16;
        const byte maxCharLength = 22;
        Check.Primitive.ForMinimumLength(value, _MinByteLength, Tag);
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value).ToBigInteger()
            ?? throw new DataElementNullException(EncodingId);

        Check.Primitive.ForMinCharLength(result.CharCount, minCharLength, Tag);
        Check.Primitive.ForMaxCharLength(result.CharCount, maxCharLength, Tag);

        return new DataStorageId(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataStorageId Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    #endregion
}