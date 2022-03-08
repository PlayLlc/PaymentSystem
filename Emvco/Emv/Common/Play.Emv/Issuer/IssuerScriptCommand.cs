using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Issuer;

/// <summary>
///     Contains a command for transmission to the ICC
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptCommand : PrimitiveValue, IEqualityComparer<IssuerScriptCommand>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x86;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public IssuerScriptCommand(BigInteger value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerScriptCommand Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static IssuerScriptCommand Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        const ushort maxByteLength = 261;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(IssuerScriptCommand)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerScriptCommand)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerScriptCommand(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IssuerScriptCommand? x, IssuerScriptCommand? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptCommand obj) => obj.GetHashCode();

    #endregion
}