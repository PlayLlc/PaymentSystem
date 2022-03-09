using Play.Emv.Icc;
using Play.Emv.Icc.ReadRecord;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd;

public record ReadRecordRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadRecordRequest));

    #endregion

    #region Constructor

    private ReadRecordRequest(TransactionSessionId transactionSessionId, CApduSignal cApduSignal) : base(cApduSignal, MessageTypeId,
        transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static ReadRecordRequest Create(TransactionSessionId sessionId, ShortFileId shortFileIdentifier) =>
        new(sessionId, ReadRecordCApduSignal.ReadAllRecords(shortFileIdentifier));

    #endregion
}