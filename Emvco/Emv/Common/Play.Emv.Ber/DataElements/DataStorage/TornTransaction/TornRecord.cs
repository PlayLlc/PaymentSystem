using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     A copy of a record from the Torn Transaction Log that is expired.Torn Record is sent to the
///     Terminal as part of the Discretionary Data.
/// </summary>
public record TornRecord : DataExchangeResponse
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8101;

    #endregion

    #region Instance Values

    private readonly RecordKey _Key;

    #endregion

    #region Constructor

    /// <exception cref="TerminalDataException"></exception>
    private TornRecord(PrimitiveValue[] values) : base(values)
    {
        PrimitiveValue? pan = values.FirstOrDefault(a => a.GetTag() == ApplicationPan.Tag);

        if (pan is null)
        {
            throw new TerminalDataException(
                $"The {nameof(TornRecord)} could not be created because the {nameof(ITlvReaderAndWriter)} did not contain the required {nameof(ApplicationPan)} object");
        }

        PrimitiveValue? sequence = values.FirstOrDefault(a => a.GetTag() == ApplicationPanSequenceNumber.Tag);

        if (sequence is null)
        {
            throw new TerminalDataException(
                $"The {nameof(TornRecord)} could not be created because the {nameof(ITlvReaderAndWriter)} did not contain the required {nameof(ApplicationPanSequenceNumber)} object");
        }

        _Key = new RecordKey((ApplicationPan) pan!, (ApplicationPanSequenceNumber) sequence);
    }

    public TornRecord(Record record) : base(record.GetValues())
    {
        _Key = record.GetRecordKey();
    }

    #endregion

    #region Instance Members

    public RecordKey GetKey() => _Key;

    /// <exception cref="Exceptions.TerminalDataException"></exception>
    public static TornRecord Create(ITlvReaderAndWriter database) => new(Record.Create(database));

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public override TornRecord Decode(TagLengthValue value) => new(_Codec.DecodePrimitiveValuesAtRuntime(value.EncodeValue().AsSpan()));

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
}