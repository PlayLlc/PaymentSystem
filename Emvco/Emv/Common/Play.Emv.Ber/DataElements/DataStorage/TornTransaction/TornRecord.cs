using System.Diagnostics.CodeAnalysis;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core;
using Play.Emv.Ber.Exceptions;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;

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
    public static readonly TornRecord Empty = CreateEmptyTornRecord();

    #endregion

    #region Instance Values

    /// <summary>
    ///     The universal time that this record was created
    /// </summary>
    protected readonly DateTimeUtc _CommitTimeStamp;

    /// <summary>
    ///     A key that uniquely identifies a Record within a defined time period. This key could potentially cause collisions
    ///     if the maximum time threshold is not adhered to
    /// </summary>
    private readonly TornEntry _Key;

    #endregion

    #region Constructor

    public TornRecord(Record record) : base(record.GetValues())
    {
        _Key = new TornEntry(record.GetKey());
        _CommitTimeStamp = DateTimeUtc.Now;
    }

    /// <exception cref="TerminalDataException"></exception>
    public TornRecord(params PrimitiveValue[] values) : base(InitializeTornEntryValues(values, out TornEntry tornEntry))
    {
        _Key = tornEntry;
        _CommitTimeStamp = DateTimeUtc.Now;
    }

    #endregion

    #region Instance Members

    [SuppressMessage("Design", "Ex0100:Member may throw undocumented exception", Justification = "<Pending>")]
    public static TornRecord CreateEmptyTornRecord()
    {
        ApplicationPan mandatoryRecordObject1 = new(new TrackPrimaryAccountNumber(Array.Empty<Nibble>()));
        ApplicationPanSequenceNumber mandatoryRecordObject2 = new(0);

        return new TornRecord(Record.Create(new PrimitiveValue[] {mandatoryRecordObject1, mandatoryRecordObject2}));
    }

    /// <exception cref="TerminalDataException"></exception>
    private static PrimitiveValue[] InitializeTornEntryValues(PrimitiveValue[] values, out TornEntry tornEntry)
    {
        Record record = Record.Create(values);
        tornEntry = new TornEntry(record.GetKey());

        return record.GetValues();
    }

    /// <exception cref="OverflowException"></exception>
    public static bool TryGetOldestRecord(TornRecord[] records, out TornRecord? result)
    {
        if (records.Length == 0)
        {
            result = null;

            return false;
        }

        if (records.Length == 1)
        {
            result = records[0];

            return true;
        }

        result = records[0];

        for (int i = 1; i < records.Length; i++)
        {
            if (records[i]._CommitTimeStamp < result._CommitTimeStamp)
                result = records[i];
        }

        return true;
    }

    public bool HasRecordExpired(Seconds timeout)
    {
        Seconds timeElapsed = DateTimeUtc.Now - _CommitTimeStamp;

        return timeElapsed >= timeout;
    }

    public TornEntry GetKey() => _Key;

    /// <exception cref="Exceptions.TerminalDataException"></exception>
    public static TornRecord Create(ITlvReaderAndWriter database) => new(Record.Create(database));

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public bool TryGetRecordItem(Tag tag, out PrimitiveValue? result)
    {
        result = _Value.FirstOrDefault(a => a.GetTag() == tag);

        return result is null;
    }

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