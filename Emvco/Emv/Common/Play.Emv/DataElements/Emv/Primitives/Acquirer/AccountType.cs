using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.Acquirer;

/// <summary>
///     Uniquely identifies the acquirer within each payment system
/// </summary>
public record AccountType : DataElement<byte>, IEqualityComparer<AccountType>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F01;
    public static readonly AccountType Default = new(0);
    public static readonly AccountType Savings = new(10);
    public static readonly AccountType Checking = new(20);
    public static readonly AccountType Credit = new(30);
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public AccountType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static AccountType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerInternalException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="DataElementNullException"></exception>
    public static AccountType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value).ToByteResult() ?? throw new DataElementNullException(EncodingId);

        return new AccountType(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(AccountType? x, AccountType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AccountType obj) => obj.GetHashCode();

    #endregion
}