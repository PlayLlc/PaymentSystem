using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

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

    #endregion

    #region Serialization

    public static DataNeeded Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override DataNeeded Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataNeeded Decode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return new DataNeeded(Array.Empty<Tag>());

        // This Value field is already BER encoded, which is what this object's _Value field requires
        return new DataNeeded(_Codec.DecodeTags(value.ToArray()));
    }

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

    /// <summary>
    ///     Searches the <see cref="IReadTlvDatabase" /> for any <see cref="Tag" /> values requested by this
    ///     <see cref="DataNeeded" />. If any of the requested values are present in the <see cref="IReadTlvDatabase" />, those
    ///     <see cref="Tag" /> values are removed from this list. The method returns number of <see cref="Tag" /> objects
    ///     requested by this <see cref="DataNeeded" /> object that were unable to be resolved by the
    ///     <see cref="IReadTlvDatabase" />
    /// </summary>
    /// <returns>
    ///     The number of requested <see cref="Tag" /> objects that were unable to be resolved by the
    ///     <see cref="IReadTlvDatabase" />
    /// </returns>
    /// <warning>
    ///     This should be the only method available to resolve the requested tags. That way we're consistent in how we're
    ///     handling data exchange
    /// </warning>
    /// <exception cref="TerminalDataException"></exception>
    public int Resolve(IReadTlvDatabase database)
    {
        for (nint i = 0; i < _Value.Count; i++)
        {
            if (!TryDequeue(out Tag tag))
                throw new TerminalDataException($"The {nameof(TagsToRead)} could not dequeue a value from memory");

            if (!database.IsPresentAndNotEmpty(tag))
                Enqueue(tag);
        }

        return _Value.Count;
    }

    #endregion
}