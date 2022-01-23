using Play.Emv.Sessions;
using Play.Icc.Emv;
using Play.Icc.Emv.ReadRecord;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordCommand : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(ReadElementaryFileRecordCommand));

    #endregion

    #region Constructor

    public ReadElementaryFileRecordCommand(CApduSignal cApduSignal, TransactionSessionId transactionSessionId) : base(cApduSignal,
     MessageTypeId, transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static ReadElementaryFileRecordCommand Create(
        TransactionSessionId transactionSessionId,
        ShortFileId shortFileId,
        RecordNumber recordNumber) =>
        new(ReadRecordCApduSignal.ReadRecord(shortFileId, recordNumber), transactionSessionId);

    #endregion
}