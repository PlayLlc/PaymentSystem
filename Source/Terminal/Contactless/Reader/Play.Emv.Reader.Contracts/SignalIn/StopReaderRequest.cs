using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalIn;

public record StopReaderRequest : RequestSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(AbortReaderRequest));
    public static readonly ChannelTypeId ChannelTypeId = ReaderChannel.Id;

    #endregion

    #region Instance Values

    private readonly DataExchangeTerminalId _DataExchangeTerminalId;

    #endregion

    #region Constructor

    public StopReaderRequest(DataExchangeTerminalId dataExchangeTerminalId) : base(MessageTypeId, ChannelTypeId)
    {
        _DataExchangeTerminalId = dataExchangeTerminalId;
    }

    #endregion

    #region Instance Members

    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;

    #endregion
}