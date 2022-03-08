using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains proprietary issuer data for transmission to the ICC before the second GENERATE AC command
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptTemplate1 : DataElement<BigInteger>, IEqualityComparer<IssuerScriptTemplate1>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x71;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;

    #endregion

    #region Constructor

    public IssuerScriptTemplate1(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    #endregion

    #region Serialization

    public static IssuerScriptTemplate1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerScriptTemplate1 Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new DataElementParsingException(
                $"The {nameof(IssuerScriptTemplate1)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerScriptTemplate1(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerScriptTemplate1? x, IssuerScriptTemplate1? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerScriptTemplate1 obj) => obj.GetHashCode();

    #endregion
}