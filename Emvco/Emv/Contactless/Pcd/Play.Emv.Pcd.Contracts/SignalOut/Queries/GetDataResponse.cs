﻿using Play.Emv.Icc.GetData;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record GetDataResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(GetDataResponse));

    #endregion

    #region Constructor

    public GetDataResponse(CorrelationId correlation, TransactionSessionId transactionSessionId, GetDataRApduSignal response) : base(
        correlation, MessageTypeId, transactionSessionId, response)
    { }

    #endregion
}