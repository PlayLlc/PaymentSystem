using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts.SignalOut;

/// <summary>
/// </summary>
public record QueryKernelResponse : ResponseSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(QueryKernelResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly DataExchangeTerminalId _DataExchangeTerminalId;
    private readonly DataToSend _DataToSend;

    #endregion

    #region Constructor

    public QueryKernelResponse(CorrelationId correlationId, DataToSend dataToSend, DataExchangeTerminalId dataExchangeTerminalId) : base(
        correlationId, MessageTypeId, ChannelTypeId)
    {
        _DataToSend = dataToSend;
        _DataExchangeTerminalId = dataExchangeTerminalId;
    }

    #endregion

    #region Instance Members

    public DataToSend GetDataToSend() => _DataToSend;
    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;

    #endregion
}