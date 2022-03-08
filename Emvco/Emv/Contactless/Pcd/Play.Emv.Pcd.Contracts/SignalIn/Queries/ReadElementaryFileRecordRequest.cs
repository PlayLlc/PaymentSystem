using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadElementaryFileRecordRequest));

    #endregion

    #region Constructor

    public ReadElementaryFileRecordRequest(CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(cApduSignal,
        MessageTypeId, transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static ReadElementaryFileRecordRequest Create(
        TransactionSessionId transactionSessionId,
        ShortFileId shortFileId,
        RecordNumber recordNumber) =>
        new(ReadRecordCApduSignal.ReadRecord(shortFileId, recordNumber), transactionSessionId);

    #endregion
}