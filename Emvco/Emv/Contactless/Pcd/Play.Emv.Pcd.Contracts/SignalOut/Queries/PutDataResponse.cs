using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record PutDataResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(PutDataResponse));

    #endregion

    #region Constructor

    public PutDataResponse(CorrelationId correlationId, TransactionSessionId transactionSessionId, PutDataRApduSignal rApdu) : base(
        correlationId, MessageTypeId, transactionSessionId, rApdu)
    { }

    #endregion
}