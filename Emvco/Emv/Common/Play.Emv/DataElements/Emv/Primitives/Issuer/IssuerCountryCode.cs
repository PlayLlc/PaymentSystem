using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record IssuerCountryCode : DataElement<ushort>, IEqualityComparer<IssuerCountryCode>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x5F28;
    private const byte _ByteLength = 2;
    private const byte _CharacterLength = 3;

    #endregion

    #region Constructor

    public IssuerCountryCode(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public static bool StaticEquals(IssuerCountryCode? x, IssuerCountryCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    #endregion

    #region Serialization

    public static IssuerCountryCode Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerCountryCode Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value) as DecodedResult<ushort>
            ?? throw new DataElementParsingException(
                $"The {nameof(IssuerCountryCode)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<ushort>)}");

        Check.Primitive.ForCharLength(result.CharCount, _CharacterLength, Tag);

        return new IssuerCountryCode(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerCountryCode? x, IssuerCountryCode? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerCountryCode obj) => obj.GetHashCode();

    #endregion
}