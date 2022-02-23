using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs; 

namespace Play.Emv.Ber.DataObjects;

/// <summary>
///     Description: List of tags included in the DEK DataExchangeSignal to request information from the Terminal.
/// </summary>
public record DataNeeded : DataExchangeRequest
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF8106;

    #endregion

    #region Constructor

    public DataNeeded() : base(Array.Empty<Tag>())
    { }

    public DataNeeded(params Tag[] tags) : base(tags)
    { }

    public DataNeeded(byte[] value) : base(_Codec.DecodeTagSequence(value))
    { }

    #endregion

    #region Equality

    public bool Equals(DataNeeded? x, DataNeeded? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(DataNeeded obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public Tag[] AsTagArray() => _Value.ToArray();
    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// TODO: What? Book C-2 section 3.6.2 says "The process continues until all records have been read" so unless there's some optimization reason that i find out about later there's not really a use for this

    //public override Tag GetNextAvailableTagFromCard() => throw new NotImplementedException();

    #endregion

    #region Serialization

    public static DataNeeded Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataNeeded Decode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return new DataNeeded(Array.Empty<byte>());

        // This Value field is already BER encoded, which is what this object's _Value field requires
        return new DataNeeded(value.ToArray());
    }

    #endregion
}