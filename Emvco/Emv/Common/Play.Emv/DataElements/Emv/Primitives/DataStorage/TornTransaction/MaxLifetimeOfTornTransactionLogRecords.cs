using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

public record MaxLifetimeOfTornTransactionLogRecords : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811C;

    #endregion

    #region Constructor

    public MaxLifetimeOfTornTransactionLogRecords(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MaxLifetimeOfTornTransactionLogRecords Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static MaxLifetimeOfTornTransactionLogRecords Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, 1, Tag);

        return new MaxLifetimeOfTornTransactionLogRecords(value[0]);
    }

    #endregion

    #region Equality

    public bool Equals(TornRecord? x, TornRecord? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TornRecord obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static explicit operator byte(MaxLifetimeOfTornTransactionLogRecords value) => value._Value;

    #endregion
}