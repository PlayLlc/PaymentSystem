using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv.Primitives.Security;

/// <summary>
///     Description: List of tags of primitive data objects defined in this specification for which the value fields must
///     be included in the static data to be signed.
/// </summary>
public record StaticDataAuthenticationTagList : DataElement<BigInteger>, IEqualityComparer<StaticDataAuthenticationTagList>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F4A;

    #endregion

    #region Constructor

    public StaticDataAuthenticationTagList(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static StaticDataAuthenticationTagList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static StaticDataAuthenticationTagList Decode(ReadOnlySpan<byte> value)
    {
        const ushort maxByteLength = 250;

        if (value.Length > maxByteLength)
        {
            throw new ArgumentOutOfRangeException(
                $"The Primitive Value {nameof(StaticDataAuthenticationTagList)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be less than {maxByteLength} bytes in length");
        }

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(StaticDataAuthenticationTagList)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new StaticDataAuthenticationTagList(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(StaticDataAuthenticationTagList? x, StaticDataAuthenticationTagList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(StaticDataAuthenticationTagList obj) => obj.GetHashCode();

    #endregion
}