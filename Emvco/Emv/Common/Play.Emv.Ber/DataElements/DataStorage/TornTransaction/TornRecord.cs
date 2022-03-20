using System.Numerics;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     A copy of a record from the Torn Transaction Log that is expired.Torn Record is sent to the
///     Terminal as part of the Discretionary Data.
/// </summary>
public record TornRecord : DataExchangeResponse, IEqualityComparer<TornRecord>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8101;

    #endregion

    #region Constructor

    public TornRecord(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsMatch(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber) => throw new NotImplementedException();

    #endregion

    #region Serialization

    public override DiscretionaryData Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    public static DiscretionaryData Decode(ReadOnlyMemory<byte> value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value).ToArray());

    /// <exception cref="BerParsingException"></exception>
    public static DiscretionaryData Decode(ReadOnlySpan<byte> value) =>
        new(_Codec.DecodePrimitiveValuesAtRuntime(value.ToArray().AsMemory()).ToArray());

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

    //if (!TryGet(pan.GetTag(), out TagLengthValue? result1))
    //    return false;
    //if (!TryGet(pan.GetTag(), out TagLengthValue? result2))
    //    return false;
    //return true;
}