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
///     Contains a command for transmission to the ICC
/// </summary>
/// <remarks>
///     Book 3 Section 10.10
/// </remarks>
public record IssuerScriptCommand : DataElement<BigInteger>, IEqualityComparer<IssuerScriptCommand>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x86;

    #endregion
     

    #region Constructor

    public IssuerScriptCommand(BigInteger value) : base(value)
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
     

    private const byte _MaxByteLength = 161;

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerScriptCommand Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);


    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static IssuerScriptCommand Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

         
        return new IssuerScriptCommand(result);
    }
     

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