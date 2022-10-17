using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes.DataStorage;
using Play.Emv.Kernel.DataExchange;
using Play.Globalization.Time;

namespace Play.Emv.Kernel.Services;

/// <summary>
///     The customer may remove the Card from the field of a Reader before the transaction has completed. The generic term
///     used for this is “tearing”, resulting in a “torn transaction”. This objects helps manage those torn transactions in
///     the event that the cardholder needs to present their card again
/// </summary>
/// <remarks>EMVco Book C-2 Section 3.8.3</remarks>
public class TornTransactionLog : IManageTornTransactions
{
    #region Instance Values

    private readonly Dictionary<TornEntry, TornRecord> _TornRecords;
    private readonly Seconds _MaxLogLifetime;
    private readonly byte _MaxNumberOfLogs;

    #endregion

    #region Constructor

    public TornTransactionLog(MaxNumberOfTornTransactionLogRecords maxRecords, MaxLifetimeOfTornTransactionLogRecords maxLifetime)
    {
        _MaxLogLifetime = maxLifetime;
        _MaxNumberOfLogs = (byte) maxRecords;
        _TornRecords = new Dictionary<TornEntry, TornRecord>();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Adds a new Torn Record to the Manager. If the number of records surpass the max number of allowed records, a
    ///     <see cref="TornRecord" /> is removed from this queue and returned in the out parameter
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    public void Add(TornRecord tornRecord, ITlvReaderAndWriter database)
    {
        CleanStaleRecords();

        if ((_TornRecords.Count + 1) > _MaxNumberOfLogs)
        {
            TornRecord.TryGetOldestRecord(_TornRecords.Values.ToArray(), out TornRecord? oldestRecord);
            database.Update(oldestRecord!);
            _TornRecords.Remove(oldestRecord!.GetKey());
        }

        _TornRecords.Add(tornRecord.GetKey(), tornRecord);
    }

    public bool TryGet(TornEntry tornEntry, out TornRecord? result)
    {
        CleanStaleRecords();

        return _TornRecords.TryGetValue(tornEntry, out result);
    }

    public void Remove(IWriteToDek dataExchangeKernel, TornEntry tornEntry)
    {
        if (!_TornRecords.ContainsKey(tornEntry))
            return;

        dataExchangeKernel.Enqueue(DekResponseType.TornRecord, _TornRecords[tornEntry]);
        _TornRecords.Remove(tornEntry);
    }

    public void CleanOldRecords(IWriteToDek dataExchangeKernel, DekResponseType dekResponseType)
    {
        foreach (KeyValuePair<TornEntry, TornRecord> record in _TornRecords)
        {
            if (record.Value.HasRecordExpired(_MaxLogLifetime))
            {
                dataExchangeKernel.Enqueue(dekResponseType, record.Value);
                _TornRecords.Remove(record.Key);
            }
        }
    }

    private void CleanStaleRecords()
    {
        foreach (KeyValuePair<TornEntry, TornRecord> record in _TornRecords)
        {
            if (record.Value.HasRecordExpired(_MaxLogLifetime))
                _TornRecords.Remove(record.Key);
        }
    }

    #endregion
}