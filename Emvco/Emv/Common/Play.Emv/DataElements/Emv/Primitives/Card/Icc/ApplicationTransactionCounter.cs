using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv.Primitives.Card.Icc;

/// <summary>
///     Counter maintained by the application in the ICC (incrementing the ATC is managed by the ICC)
/// </summary>
public record ApplicationTransactionCounter : DataElement<ushort>, IEqualityComparer<ApplicationTransactionCounter>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F36;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public ApplicationTransactionCounter(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static ApplicationTransactionCounter Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ApplicationTransactionCounter Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 2;

        if (value.Length != byteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(ApplicationTransactionCounter)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value) as DecodedResult<ushort>
            ?? throw new InvalidOperationException(
                $"The {nameof(ApplicationTransactionCounter)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        return new ApplicationTransactionCounter(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(ApplicationTransactionCounter? x, ApplicationTransactionCounter? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ApplicationTransactionCounter obj) => obj.GetHashCode();

    #endregion
}