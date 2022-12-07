using System.Collections.Generic;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes.DataStorage;
using Play.Emv.Kernel.Services;

namespace Play.Emv.Kernel.Databases;

public class ScratchPad
{
    #region Instance Values

    private readonly Dictionary<ApplicationIdentifier, TornTransactionLog> _TornTransactionLogs;
    private readonly MaxNumberOfTornTransactionLogRecords _MaxNumberOfTornTransactionLogRecords;
    private readonly MaxLifetimeOfTornTransactionLogRecords _MaxLifetimeOfTornTransactionLogRecords;

    #endregion

    #region Constructor

    public ScratchPad(
        MaxNumberOfTornTransactionLogRecords maxNumberOfTornTransactionLogRecords,
        MaxLifetimeOfTornTransactionLogRecords maxLifetimeOfTornTransactionLogRecords)
    {
        _TornTransactionLogs = new Dictionary<ApplicationIdentifier, TornTransactionLog>();
        _MaxNumberOfTornTransactionLogRecords = maxNumberOfTornTransactionLogRecords;
        _MaxLifetimeOfTornTransactionLogRecords = maxLifetimeOfTornTransactionLogRecords;
    }

    #endregion

    #region Instance Members

    public TornTransactionLog GetTornTransactionLog(ApplicationIdentifier applicationId)
    {
        if (!_TornTransactionLogs.ContainsKey(applicationId))
            _TornTransactionLogs.Add(applicationId, new TornTransactionLog(_MaxNumberOfTornTransactionLogRecords, _MaxLifetimeOfTornTransactionLogRecords));

        return _TornTransactionLogs[applicationId]!;
    }

    /// <exception cref="TerminalDataException"></exception>
    public void Add(ITlvReaderAndWriter database, ApplicationIdentifier applicationId, TornRecord tornRecord)
    {
        if (!_TornTransactionLogs.ContainsKey(applicationId))
            _TornTransactionLogs.Add(applicationId, new TornTransactionLog(_MaxNumberOfTornTransactionLogRecords, _MaxLifetimeOfTornTransactionLogRecords));

        _TornTransactionLogs[applicationId].Add(tornRecord, database);
    }

    public bool TryGet(ApplicationIdentifier applicationId, TornEntry tornEntry, out TornRecord? result) =>
        _TornTransactionLogs[applicationId].TryGet(tornEntry, out result);

    #endregion
}