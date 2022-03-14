using Play.Emv.Icc;
using Play.Emv.Icc.ReadRecord;
using Play.Emv.Sessions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadRecordRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadRecordRequest));

    #endregion

    #region Instance Values

    private readonly ShortFileId _ShortFileId;

    #endregion

    #region Constructor

    private ReadRecordRequest(TransactionSessionId transactionSessionId, ShortFileId shortFileId, CApduSignal cApduSignal) :
        base(cApduSignal, MessageTypeId, transactionSessionId)
    {
        _ShortFileId = shortFileId;
    }

    #endregion

    #region Instance Members

    public ShortFileId GetShortFileId() => _ShortFileId;

    public static ReadRecordRequest Create(TransactionSessionId sessionId, ShortFileId shortFileIdentifier) =>
        new(sessionId, shortFileIdentifier, ReadRecordCApduSignal.ReadAllRecords(shortFileIdentifier));

    #endregion
}