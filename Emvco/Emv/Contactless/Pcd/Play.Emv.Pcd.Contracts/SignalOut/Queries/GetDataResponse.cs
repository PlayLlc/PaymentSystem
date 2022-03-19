using System;

using Play.Ber.DataObjects;
using Play.Emv.Ber;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetDataResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetDataResponse));

    #endregion

    #region Constructor

    public GetDataResponse(CorrelationId correlation, TransactionSessionId transactionSessionId, GetDataRApduSignal response) :
        base(correlation, MessageTypeId, transactionSessionId, response)
    { }

    #endregion

    #region Instance Members

    public TagLengthValue GetTagLengthValueResult() => EmvCodec.GetBerCodec().DecodeTagLengthValue(GetRApduSignal().GetData().AsSpan());
     

    #endregion
}