using System.Linq;

using Play.Ber.DataObjects;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

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

    public TagLengthValue[] GetTagLengthValuesResult()
    {
        TagLengthValue[] result = new TagLengthValue[_DataBatchResponses.Length];

        for (nint i = 0; i < _DataBatchResponses.Length; i++)
            result[i] = _DataBatchResponses[i].GetTagLengthValueResult();

        return result;
    }

    #endregion
}