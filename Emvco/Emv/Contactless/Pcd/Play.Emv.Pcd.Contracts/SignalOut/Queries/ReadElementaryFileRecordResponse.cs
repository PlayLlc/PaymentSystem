using Play.Emv.Icc.ReadRecord;
using Play.Emv.Sessions;
using Play.Emv.Templates;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadElementaryFileRecordResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadElementaryFileRecordResponse));

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
    public byte[] GetRawRecord() => GetData();
    public int GetValueByteCount() => GetData().Length;

    #endregion
}