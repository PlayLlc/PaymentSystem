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
///     Specifies the issuer�s conditions that cause a transaction to be rejected if it might have been approved online,
///     but the terminal is unable to process the transaction online
/// </summary>
public record IssuerActionCodeDefault : DataElement<ulong>, IEqualityComparer<IssuerActionCodeDefault>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F0D;

    #endregion

    #region Constructor

    public IssuerActionCodeDefault(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public ActionCodes AsActionCodes() => new ActionCodes(_Value);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerActionCodeDefault Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerActionCodeDefault Decode(ReadOnlySpan<byte> value)
    {
        const ushort byteLength = 5;

        if (value.Length != byteLength)
        {
            throw new DataElementParsingException(
                $"The Primitive Value {nameof(IssuerActionCodeDefault)} could not be initialized because the byte length provided was out of range. The byte length was {value.Length} but must be {byteLength} bytes in length");
        }

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value) as DecodedResult<ulong>
            ?? throw new DataElementParsingException(
                $"The {nameof(IssuerActionCodeDefault)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new IssuerActionCodeDefault(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerActionCodeDefault? x, IssuerActionCodeDefault? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerActionCodeDefault obj) => obj.GetHashCode();

    #endregion
}