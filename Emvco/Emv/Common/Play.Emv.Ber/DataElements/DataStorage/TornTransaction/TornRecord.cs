﻿using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Services;
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

        _Key = new TornEntry((ApplicationPan) pan!, (ApplicationPanSequenceNumber) sequence);
        _CommitTimeStamp = DateTimeUtc.Now();
    }

    public TornRecord(Record record) : base(record.GetValues())
    {
        _Key = new TornEntry(record.GetRecordKey());
        _CommitTimeStamp = DateTimeUtc.Now();
    }

    public TornRecord(params PrimitiveValue[] values) : base(values)
    {
        var a = Record.Create()
    }

    private static void Create

    #endregion

    #region Instance Members

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
        Seconds timeElapsed = DateTimeUtc.Now() - _CommitTimeStamp;

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