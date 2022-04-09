﻿using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Kernel.DataExchange;

public interface IDekEndpointClient
{
    #region Instance Members

    public void SendRequest(KernelSessionId sessionId);
    public void SendResponse(KernelSessionId sessionId, CorrelationId correlationId);

    #endregion
}