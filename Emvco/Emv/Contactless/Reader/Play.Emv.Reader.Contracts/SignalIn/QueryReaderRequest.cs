using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalIn;

public record QueryReaderRequest : RequestSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(QueryReaderRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Reader;

    #endregion

    #region Instance Values

    private readonly TagsToRead _TagsToRead;
    private readonly DataExchangeTerminalId _DataExchangeTerminalId;

    #endregion

    #region Constructor

    public QueryReaderRequest(DataExchangeTerminalId dataExchangeTerminalId, TagsToRead tagsToRead) : base(MessageTypeId, ChannelTypeId)
    {
        _DataExchangeTerminalId = dataExchangeTerminalId;
        _TagsToRead = tagsToRead;
    }

    #endregion

    #region Instance Members

    public TagsToRead GetTagsToRead() => _TagsToRead;
    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;

    #endregion
}