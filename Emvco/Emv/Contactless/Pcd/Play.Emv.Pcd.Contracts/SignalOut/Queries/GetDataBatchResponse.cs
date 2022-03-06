using System.Linq;

using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.GetData;

public record GetDataBatchResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetDataBatchResponse));

    #endregion

    #region Instance Values

    private readonly GetDataResponse[] _DataBatchResponses;

    #endregion

    #region Constructor

    public GetDataBatchResponse(
        CorrelationId correlationId,
        TransactionSessionId transactionSessionId,
        GetDataResponse[] dataBatchResponses) : base(correlationId, MessageTypeId, transactionSessionId,
        dataBatchResponses.FirstOrDefault()!.GetRApduSignal())
    {
        _DataBatchResponses = dataBatchResponses;
    }

    #endregion

    #region Instance Members

    public GetDataResponse[] GetDataBatch() => _DataBatchResponses;

    #endregion
}