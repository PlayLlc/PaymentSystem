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
///     Contains proprietary issuer data for transmission to the ICC after the second GENERATE AC command
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptTemplate2 : PrimitiveValue, IEqualityComparer<IssuerScriptTemplate2>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x72;

    #endregion

    #region Instance Values

    private readonly BigInteger _Value;

    #endregion

    #region Constructor

    public IssuerScriptTemplate2(BigInteger value)
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

    public static IssuerScriptTemplate2 Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerScriptTemplate2 Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerScriptTemplate2)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerScriptTemplate2(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(EncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(EncodingId, _Value, length);

    #endregion

    #region Equality

    public bool Equals(IssuerScriptTemplate2? x, IssuerScriptTemplate2? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptTemplate2 obj) => obj.GetHashCode();

    #endregion
}