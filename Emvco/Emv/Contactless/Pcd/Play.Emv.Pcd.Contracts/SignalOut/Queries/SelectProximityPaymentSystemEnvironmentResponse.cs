﻿using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Pcd.Contracts;

public record SelectProximityPaymentSystemEnvironmentResponse : QueryPcdResponse
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(SelectProximityPaymentSystemEnvironmentResponse));

    #endregion

    #region Constructor

    public SelectProximityPaymentSystemEnvironmentResponse(
        CorrelationId correlationId, TransactionSessionId transactionSessionId, GetFileControlInformationRApduSignal responseApduSignal) :
        base(correlationId, MessageTypeId, transactionSessionId, responseApduSignal)
    { }

    #endregion

    #region Instance Members

    public bool TryGetFileControlInformation(out FileControlInformationPpse? result)
    {
        if (GetData().Length == 0)
        {
            result = null;

            return false;
        }

        result = FileControlInformationPpse.Decode(GetData());

        return true;
    }

    #endregion
}