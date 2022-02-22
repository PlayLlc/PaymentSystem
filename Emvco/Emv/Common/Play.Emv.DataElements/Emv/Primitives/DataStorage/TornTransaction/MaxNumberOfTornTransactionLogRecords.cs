using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Indicates the maximum number of records that can be stored in the Torn Transaction Log.
/// </summary>
public record MaxNumberOfTornTransactionLogRecords : DataElement<byte>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF811D;

    #endregion

    #region Constructor

    public MaxNumberOfTornTransactionLogRecords(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => PlayEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static MaxNumberOfTornTransactionLogRecords Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static MaxNumberOfTornTransactionLogRecords Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, 1, Tag);

        return new MaxNumberOfTornTransactionLogRecords(value[0]);
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

    public static explicit operator byte(MaxNumberOfTornTransactionLogRecords value) => value._Value;

    #endregion
}