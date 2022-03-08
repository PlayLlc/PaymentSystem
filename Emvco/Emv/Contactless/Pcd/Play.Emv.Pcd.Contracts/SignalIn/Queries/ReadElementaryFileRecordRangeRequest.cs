using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordRangeRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadElementaryFileRecordRangeRequest));

    #endregion

    #region Instance Values

    private readonly RecordRange _RecordRange;

    #endregion

    #region Constructor

    private ReadElementaryFileRecordRangeRequest(TransactionSessionId transactionSessionId, RecordRange recordRange) : base(default,
        MessageTypeId, transactionSessionId)
    {
        _RecordRange = recordRange;
    }

    #endregion

    #region Instance Members

    public static ReadElementaryFileRecordRangeRequest Create(TransactionSessionId transactionSessionId, RecordRange recordRange) =>
        new(transactionSessionId, recordRange);

    public RecordRange GetRecordRange() => _RecordRange;
    public ShortFileId GetShortFileId() => _RecordRange.GetShortFileIdentifier();

    #endregion
}