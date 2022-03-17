﻿using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectProximityPaymentSystemEnvironmentRequest : QueryPcdRequest
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SelectProximityPaymentSystemEnvironmentRequest));

    #endregion

    #region Constructor

    private SelectProximityPaymentSystemEnvironmentRequest(CApduSignal cApduSignal, TransactionSessionId transactionSessionId) :
        base(cApduSignal, MessageTypeId, transactionSessionId)
    { }

    #endregion

    #region Instance Members

    public static SelectProximityPaymentSystemEnvironmentRequest Create(TransactionSessionId transactionSessionId)
    {
        GetFileControlInformationCApduSignal cApdu = GetFileControlInformationCApduSignal.GetProximityPaymentSystemEnvironment();

        return new SelectProximityPaymentSystemEnvironmentRequest(cApdu, transactionSessionId);
    }

    #endregion
}