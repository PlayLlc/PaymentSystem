using Play.Emv.Sessions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordRangeCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadElementaryFileRecordRangeCommand));

    #endregion

    #region Instance Values

    private readonly RecordRange _RecordRange;

    #endregion

    #region Constructor

    private ReadElementaryFileRecordRangeCommand(TransactionSessionId transactionSessionId, RecordRange recordRange) : base(default,
        MessageTypeId, transactionSessionId)
    {
        _RecordRange = recordRange;
    }

    #endregion

    #region Instance Members

    public static ReadElementaryFileRecordRangeCommand Create(TransactionSessionId transactionSessionId, RecordRange recordRange) =>
        new(transactionSessionId, recordRange);

    public RecordRange GetRecordRange() => _RecordRange;
    public ShortFileId GetShortFileId() => _RecordRange.GetShortFileIdentifier();

    #endregion
}