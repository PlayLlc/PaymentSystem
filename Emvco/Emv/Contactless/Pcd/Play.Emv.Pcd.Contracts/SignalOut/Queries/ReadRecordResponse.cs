using Play.Ber.DataObjects;
using Play.Emv.Icc;
using Play.Emv.Icc.ReadRecord;
using Play.Emv.Sessions;
using Play.Emv.Templates;
using Play.Icc.FileSystem.ElementaryFiles;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record ReadRecordResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(ReadRecordResponse));

    #endregion

    #region Instance Values

    private readonly ShortFileId _ShortFileId;

    #endregion

    #region Constructor

    public ReadRecordResponse(CorrelationId correlationId, TransactionSessionId transactionSessionId, ReadRecordRApduSignal rApdu) :
        base(correlationId, MessageTypeId, transactionSessionId, rApdu)
    { }

    #endregion

    #region Instance Members

    public ShortFileId GetShortFileId() => _ShortFileId;
    public TagLengthValue[] GetRecords() => ReadRecordResponseTemplate.GetRecords(GetData());
    public int GetValueByteCount() => GetData().Length;

    #endregion
}