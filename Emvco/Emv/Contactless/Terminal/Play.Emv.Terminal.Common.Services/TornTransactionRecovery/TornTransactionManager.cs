﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.DataElements;
using Play.Emv.DataElements.Primitives.DataStorage.TornTransaction;
using Play.Emv.Kernel2.StateMachine;
using Play.Encryption.Hashing;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Common.Services.TornTransactionRecovery;

/// <summary>
///     The customer may remove the Card from the field of a Reader before the transaction has completed. The generic term
///     used for this is “tearing”, resulting in a “torn transaction”. This objects helps manage those torn transactions in
///     the event that the cardholder needs to present their card again
/// </summary>
internal class TornTransactionManager : ICleanTornTransactions
{
    #region Instance Values

    private readonly Queue<TornRecord> _TornRecords;
    private readonly Seconds _MaxLogLifetime;
    private readonly byte _MaxNumberOfLogs;

    #endregion

    #region Constructor

    public TornTransactionManager(MaxNumberOfTornTransactionLogRecords maxRecords, MaxLifetimeOfTornTransactionLogRecords maxLifetime)
    {
        _MaxLogLifetime = new Seconds((byte) maxLifetime);
        _MaxNumberOfLogs = (byte) maxRecords;
        _TornRecords = new Queue<TornRecord>();
    }

    #endregion

    #region Instance Members

    public TornRecord? AddAndDisplace(TornRecord value)
    {
        if (_TornRecords.Count <= _MaxNumberOfLogs)
            return null;

        _TornRecords.Enqueue(value);

        return _TornRecords.Dequeue();
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

    public TornRecord[]? Truncate()
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