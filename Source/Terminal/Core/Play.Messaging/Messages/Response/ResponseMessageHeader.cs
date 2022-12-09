﻿namespace Play.Messaging;

internal record ResponseMessageHeader : RequestMessageHeader
{
    #region Instance Values

    private readonly CorrelationId _CorrelationId;

    #endregion

    #region Constructor

    public ResponseMessageHeader(CorrelationId correlationId) : base(correlationId)
    {
        _CorrelationId = correlationId;
    }

    #endregion

    #region Instance Members

    public CorrelationId GetCorrelationId() => _CorrelationId;

    #endregion
}