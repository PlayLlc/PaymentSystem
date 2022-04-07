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

    #region Serialization

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    public override DiscretionaryData Decode(TagLengthValue value)
    {
        throw new NotImplementedException();

        // TODO: Need to create this record as a list of Data Objects in EMV Book C-2 Table 4.2

        return Decode(value.EncodeValue().AsSpan());
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public static DiscretionaryData Decode(ReadOnlyMemory<byte> value)
    {
        throw new NotImplementedException();

        // TODO: Need to create this record as a list of Data Objects in EMV Book C-2 Table 4.2

        return new DiscretionaryData(_Codec.DecodePrimitiveValuesAtRuntime(value.Span).ToArray());
    }

    /// <exception cref="BerParsingException"></exception>
    public static DiscretionaryData Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value).ToArray());

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

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsMatch(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber) => throw new NotImplementedException();

    #endregion
}