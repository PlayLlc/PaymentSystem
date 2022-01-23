using Play.Emv.Sessions;
using Play.Emv.Templates.Records;
using Play.Icc.Emv.ReadRecord;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(ReadElementaryFileRecordResponse));

    #endregion

    #region Constructor

    public ReadElementaryFileRecordResponse(
        CorrelationId correlationId,
        TransactionSessionId transactionSessionId,
        ReadRecordRApduSignal rApdu) : base(correlationId, MessageTypeId, transactionSessionId, rApdu)
    { }

    #endregion

    #region Instance Members

    public ReadRecordResponse GetReadRecordResponseTemplate() => new(GetData());

    #endregion
}