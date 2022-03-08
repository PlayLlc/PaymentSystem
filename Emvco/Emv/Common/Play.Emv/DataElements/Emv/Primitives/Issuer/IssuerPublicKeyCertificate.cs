﻿using System;
using System.Collections.Generic;
using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Provided by the appropriate certification  authority to the card issuer. When the terminal verifies this data
///     element, it authenticates the Issuer Public Key plus additional data as described in Book C Section 5.3.
/// </summary>
public record IssuerPublicKeyCertificate : DataElement<BigInteger>, IEqualityComparer<IssuerPublicKeyCertificate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x90;

    #endregion

    #region Constructor

    public IssuerPublicKeyCertificate(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.ToByteArray();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.GetByteCount();
    public ReadOnlySpan<byte> GetEncipherment() => _Value.ToByteArray().AsSpan();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static IssuerPublicKeyCertificate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static IssuerPublicKeyCertificate Decode(ReadOnlySpan<byte> value)
    {
        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(IssuerPublicKeyCertificate)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new IssuerPublicKeyCertificate(result.Value);
    }

    #endregion

    #region Equality

    public bool Equals(IssuerPublicKeyCertificate? x, IssuerPublicKeyCertificate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(IssuerPublicKeyCertificate obj) => obj.GetHashCode();

    #endregion
}