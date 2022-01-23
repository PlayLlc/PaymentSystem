﻿using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace Play.Emv.DataElements;

/// <summary>
///     Value to provide variability and uniqueness to the generation of a cryptogram
/// </summary>
public record UnpredictableNumber : DataElement<uint>, IEqualityComparer<UnpredictableNumber>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F37;
    private const byte _ByteCount = 4;

    #endregion

    #region Constructor

    public UnpredictableNumber(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ushort GetByteCount() => _ByteCount;
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static UnpredictableNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static UnpredictableNumber Decode(ReadOnlySpan<byte> value)
    {
        if (value.Length != _ByteCount)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(UnpredictableNumber)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {_ByteCount} bytes in length");
        }

        DecodedResult<uint> result = _Codec.Decode(BerEncodingId, value) as DecodedResult<uint>
            ?? throw new InvalidOperationException(
                $"The {nameof(UnpredictableNumber)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<uint>)}");

        return new UnpredictableNumber(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

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