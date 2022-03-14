using Play.Emv.Ber.DataObjects;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

public record QueryKernelResponse : ResponseSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(QueryKernelResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly DataExchangeTerminalId _DataExchangeTerminalId;
    private readonly DataToSend _DataToSend;

    #endregion

    #region Constructor

    public QueryKernelResponse(CorrelationId correlationId, DataToSend dataToSend, DataExchangeTerminalId dataExchangeTerminalId) :
        base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _DataToSend = dataToSend;
        _DataExchangeTerminalId = dataExchangeTerminalId;
    }

    #endregion

    #region Instance Members

    public DataToSend GetDataToSend() => _DataToSend;
    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;
    public TransactionSessionId GetTransactionSessionId() => _DataExchangeTerminalId.GetTransactionSessionId();

    #endregion
}