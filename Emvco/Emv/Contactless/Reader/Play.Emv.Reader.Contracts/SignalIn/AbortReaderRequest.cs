using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalIn;

public record AbortReaderRequest : RequestSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(AbortReaderRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Reader;

    #endregion

    #region Instance Values

    private readonly DataExchangeTerminalId _DataExchangeTerminalId;

    #endregion

    #region Constructor

    public AbortReaderRequest(DataExchangeTerminalId dataExchangeTerminalId) : base(MessageTypeId, ChannelTypeId)
    {
        _DataExchangeTerminalId = dataExchangeTerminalId;
    }

    #endregion

    #region Instance Members

    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;

    #endregion
}