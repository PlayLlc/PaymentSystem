using System.Collections.Immutable;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Services;

namespace Play.Emv.Kernel.Databases;

public class ScratchPad
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<ApplicationIdentifier, TornTransactionLog> _TornTransactionLogs;
    private readonly MaxNumberOfTornTransactionLogRecords _MaxNumberOfTornTransactionLogRecords;
    private readonly MaxLifetimeOfTornTransactionLogRecords _MaxLifetimeOfTornTransactionLogRecords;

    #endregion

    #region Constructor

    public ScratchPad(
        ImmutableSortedDictionary<ApplicationIdentifier, TornTransactionLog> tornTransactionLogs,
        MaxNumberOfTornTransactionLogRecords maxNumberOfTornTransactionLogRecords,
        MaxLifetimeOfTornTransactionLogRecords maxLifetimeOfTornTransactionLogRecords)
    {
        _TornTransactionLogs = tornTransactionLogs;
        _MaxNumberOfTornTransactionLogRecords = maxNumberOfTornTransactionLogRecords;
        _MaxLifetimeOfTornTransactionLogRecords = maxLifetimeOfTornTransactionLogRecords;
    }

    #endregion

    #region Instance Members

    public TornTransactionLog GetTornTransactionLog(ApplicationIdentifier applicationId)
    {
        if (!_TornTransactionLogs.TryGetValue(applicationId, out TornTransactionLog? tornTransactionLog))
        {
            _TornTransactionLogs.Add(applicationId,
                new TornTransactionLog(_MaxNumberOfTornTransactionLogRecords, _MaxLifetimeOfTornTransactionLogRecords));
        }

        return tornTransactionLog!;
    }

    public void Add(ITlvReaderAndWriter database, ApplicationIdentifier applicationId, TornRecord tornRecord) =>
        _TornTransactionLogs[applicationId].Add(tornRecord, database);

    public bool TryGet(ApplicationIdentifier applicationId, TornEntry tornEntry, out TornRecord? result) =>
        _TornTransactionLogs[applicationId].TryGet(tornEntry, out result);

    #endregion
}