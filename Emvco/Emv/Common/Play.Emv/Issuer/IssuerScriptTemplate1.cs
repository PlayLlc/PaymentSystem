using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.Issuer;

/// <summary>
///     Contains proprietary issuer data for transmission to the ICC before the second GENERATE AC command
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptTemplate1 : DataElement<BigInteger>, IEqualityComparer<IssuerScriptTemplate1>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x71;

    #endregion 

    #region Constructor

    public IssuerScriptTemplate1(BigInteger value) : base(value)
    { 
    }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization
     

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerScriptTemplate1 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerScriptTemplate1 Decode(ReadOnlySpan<byte> value)
    { 

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);


        return new IssuerScriptTemplate1(result);
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