using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Extensions;
using Play.Emv.DataElements;

namespace Play.Emv.Security.Cryptograms;

/// <summary>
///     Indicates the type of cryptogram and the actions to be performed by the terminal
/// </summary>
public record CryptogramInformationData : PrimitiveValue, IEqualityComparer<CryptogramInformationData>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F27;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public CryptogramInformationData(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId()
    {
        return BerEncodingId;
    }

    /// <summary>
    ///     GetCryptogramType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public CryptogramType GetCryptogramType()
    {
        if (!CryptogramType.TryGet(_Value, out CryptogramType? result))
        {
            throw new
                InvalidOperationException($"The {nameof(CryptogramInformationData)} expected a {nameof(CryptogramType)} but none could be found");
        }

        return result!;
    }

    public override Tag GetTag()
    {
        return Tag;
    }

    public override ushort GetValueByteCount(BerCodec codec)
    {
        return codec.GetByteCount(GetBerEncodingId(), _Value);
    }

    /// <summary>
    ///     Value signifying whether a Combined Data Authentication Signature has been requested by the ICC
    /// </summary>
    /// <returns></returns>
    public bool IsCdaSignatureRequested()
    {
        return _Value.IsBitSet(Bits.Five);
    }

    #endregion

    #region Serialization

    public static CryptogramInformationData Decode(ReadOnlyMemory<byte> value, BerCodec codec)
    {
        return Decode(value.Span, codec);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CryptogramInformationData Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort byteLength = 1;

        if (value.Length != byteLength)
        {
            throw new
                ArgumentOutOfRangeException($"The Primitive Value {nameof(CryptogramInformationData)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<byte> result = codec.Decode(BerEncodingId, value) as DecodedResult<byte>
            ?? throw new
                InvalidOperationException($"The {nameof(CryptogramInformationData)} could not be initialized because the {nameof(UnsignedBinaryCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new CryptogramInformationData(result.Value);
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

    public bool Equals(CryptogramInformationData? x, CryptogramInformationData? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(CryptogramInformationData obj)
    {
        return obj.GetHashCode();
    }

    #endregion
}