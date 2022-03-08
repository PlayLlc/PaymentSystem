using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     The number that identifies the major industry and the card issuer and that forms the first part of the Primary
///     Account Number (PAN)
/// </summary>
public record IssuerIdentificationNumber : DataElement<uint>, IEqualityComparer<IssuerIdentificationNumber>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x42;

    #endregion

    #region Constructor

    public IssuerIdentificationNumber(uint value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerIdentificationNumber Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static IssuerIdentificationNumber Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 3;
        const ushort charLength = 6;

        Check.Primitive.ForExactLength(value, byteLength, Tag);

        DecodedResult<uint> result = _Codec.Decode(EncodingId, value).ToUInt32Result() ?? throw new DataObjectParsingException(EncodingId);

        Check.Primitive.ForCharLength(result.CharCount, charLength, Tag);

        return new IssuerIdentificationNumber(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerIdentificationNumber? x, IssuerIdentificationNumber? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerIdentificationNumber obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator uint(IssuerIdentificationNumber value) => value._Value;

    #endregion
}