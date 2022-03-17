using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalOut;

/// <summary>
/// </summary>
public record QueryReaderResponse : ResponseSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(QueryReaderResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Reader;

    #endregion

    #region Instance Values

    private readonly DataExchangeTerminalId _DataExchangeTerminalId;
    private readonly DataToSend _DataToSend;

    #endregion

    #region Constructor

    public QueryReaderResponse(CorrelationId correlationId, DataToSend dataToSend, DataExchangeTerminalId dataExchangeTerminalId) :
        base(correlationId, MessageTypeId, ChannelTypeId)
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