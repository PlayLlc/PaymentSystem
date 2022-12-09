﻿using Play.Emv.Ber.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Terminal.Contracts.SignalOut;

/// <summary>
/// </summary>
public record QueryTerminalResponse : ResponseSignal, IExchangeDataWithTheKernel
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(QueryTerminalResponse));
    public static readonly ChannelTypeId ChannelTypeId = TerminalChannel.Id;

    #endregion

    #region Instance Values

    private readonly DataExchangeKernelId _DataExchangeKernelId;
    private readonly DataToSend _DataToSend;

    #endregion

    #region Constructor

    public QueryTerminalResponse(CorrelationId correlationId, DataToSend dataToSend, DataExchangeKernelId dataExchangeKernelId) : base(correlationId,
        MessageTypeId, ChannelTypeId)
    {
        _DataToSend = dataToSend;
        _DataExchangeKernelId = dataExchangeKernelId;
    }

    #endregion

    #region Instance Members

    public KernelId GetKernelId() => _DataExchangeKernelId.GetShortKernelId();
    public DataToSend GetDataToSend() => _DataToSend;
    public DataExchangeKernelId GetDataExchangeKernelId() => _DataExchangeKernelId;
    public KernelSessionId GetKernelSessionId() => _DataExchangeKernelId.GetKernelSessionId();
    public TransactionSessionId GetTransactionSessionId() => _DataExchangeKernelId.GetTransactionSessionId();

    #endregion
}