using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;

namespace ___TEMP.Play.Emv.Security.Cryptograms;

/// <summary>
///     Digital signature on critical application parameters for DDA or CDA
/// </summary>
public record SignedDynamicApplicationData : PrimitiveValue, IEqualityComparer<SignedDynamicApplicationData>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F4B;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public SignedDynamicApplicationData(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public byte[] AsByteArray()
    {
        return _Value.ToByteArray();
    }

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    public ushort GetByteCount()
    {
        return (ushort) _Value.GetByteCount();
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    #endregion

    #region Serialization

    public static SignedDynamicApplicationData Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static SignedDynamicApplicationData Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new
                InvalidOperationException($"The {nameof(SignedDynamicApplicationData)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new SignedDynamicApplicationData(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec)
    {
        return codec.EncodeValue(BerEncodingId, _Value);
    }

    public override byte[] EncodeValue(BerCodec codec, int length)
    {
        return codec.EncodeValue(BerEncodingId, _Value, length);
    }

    #endregion

    #region Equality

    public bool Equals(SignedDynamicApplicationData? x, SignedDynamicApplicationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(SignedDynamicApplicationData obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}