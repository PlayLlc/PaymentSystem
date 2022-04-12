using System;
using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Time;
using Play.Globalization.Time.Seconds;

namespace Play.Emv.Kernel.Services;

/// <summary>
///     The customer may remove the Card from the field of a Reader before the transaction has completed. The generic term
///     used for this is “tearing”, resulting in a “torn transaction”. This objects helps manage those torn transactions in
///     the event that the cardholder needs to present their card again
/// </summary>
/// <remarks>EMVco Book C-2 Section 3.8.3</remarks>
internal class TornTransactionManager : IManageTornTransactions
{
    #region Instance Values

    private readonly Queue<TornRecord> _TornRecords;
    private readonly Seconds _MaxLogLifetime;
    private readonly byte _MaxNumberOfLogs;

    #endregion

    #region Constructor

    public TornTransactionManager(MaxNumberOfTornTransactionLogRecords maxRecords, MaxLifetimeOfTornTransactionLogRecords maxLifetime)
    {
        _MaxLogLifetime = maxLifetime;
        _MaxNumberOfLogs = (byte) maxRecords;
        _TornRecords = new Queue<TornRecord>();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Adds a new Torn Record to the Manager. If the number of records surpass the max number of allowed records, a
    ///     <see cref="TornRecord" /> is removed from this queue and returned in the out parameter
    /// </summary>
    /// <param name="database"></param>
    /// <param name="displacedRecord">
    ///     This value is not null if a record is added and the maximum number of records already
    ///     exist
    /// </param>
    /// <exception cref="TerminalDataException"></exception>
    public bool TryAddAndDisplace(ITlvReaderAndWriter database, out TornRecord? displacedRecord)
    {
        TornRecord currentRecord = TornRecord.Create(database);
        _TornRecords.Enqueue(currentRecord);

        if (_TornRecords.Count >= _MaxNumberOfLogs)
        {
            displacedRecord = _TornRecords.Dequeue();

            return true;
        }

        displacedRecord = null;

        return false;
    }

    public bool TryGet(ApplicationPan pan, ApplicationPanSequenceNumber sequenceNumber, out TornRecord? result)
    {
        result = _TornRecords.FirstOrDefault(a => a.IsMatch(pan, sequenceNumber));

        return result is null;
    }

    public bool TryDequeue(out TornRecord? result)
    {
        if (_TornRecords.Count == 0)
        {
            result = null;

            return false;
        }

        result = _TornRecords.Dequeue();

        return true;
    }

    public void Clean() => _TornRecords.Clear();

    public TornRecord[]? Truncate(KernelDatabase database)
    {
        if (_TornRecords.Count <= _MaxNumberOfLogs)
            return null;

        TornRecord[] result = new TornRecord[_MaxNumberOfLogs - _TornRecords.Count];

        for (int i = 0; i < (_MaxNumberOfLogs - result.Length); i++)
            _ = _TornRecords.Dequeue();

        return result;
    }

    #endregion
}