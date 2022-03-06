using Play.Emv.Icc;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SendPoiInformationResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SendPoiInformationResponse));

    #endregion

    #region Constructor

    public SendPoiInformationResponse(CorrelationId correlation, TransactionSessionId transactionSessionId, RApduSignal responseApdu) :
        base(correlation, MessageTypeId, transactionSessionId, responseApdu)
    { }

    #endregion
}