﻿using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Randoms;

namespace Play.Emv.DataElements;

/// <summary>
///     Value to provide variability and uniqueness to the generation of a cryptogram
/// </summary>
public record UnpredictableNumber : DataElement<uint>, IEqualityComparer<UnpredictableNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F37;
    private const byte _ByteCount = 4;

    #endregion

    #region Constructor

    // WARNING - This should be generated by secure hardware like DUKPT
    public UnpredictableNumber() : base(Randomize.Numeric.UInt())
    { }

    public UnpredictableNumber(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ushort GetByteCount() => _ByteCount;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static UnpredictableNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static UnpredictableNumber Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteCount)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(UnpredictableNumber)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteCount} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(EncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(UnpredictableNumber)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new UnpredictableNumber(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

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
}