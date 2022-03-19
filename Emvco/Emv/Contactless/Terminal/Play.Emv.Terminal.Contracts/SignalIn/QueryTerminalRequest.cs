using Play.Emv.Ber.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Terminal.Contracts.SignalIn;

public record QueryTerminalRequest : RequestSignal, IExchangeDataWithTheKernel
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(QueryTerminalRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

    #endregion

    #region Instance Values

    private readonly DataNeeded _DataNeeded;
    private readonly DataExchangeKernelId _DataExchangeKernelId;

    #endregion

    #region Constructor

    public QueryTerminalRequest(DataExchangeKernelId dataExchangeKernelId, DataNeeded dataNeeded) : base(MessageTypeId, ChannelTypeId)
    {
        _DataExchangeKernelId = dataExchangeKernelId;
        _DataNeeded = dataNeeded;
    }

    #endregion

    #region Instance Members

    public DataNeeded GetDataNeeded() => _DataNeeded;
    public DataExchangeKernelId GetDataExchangeKernelId() => _DataExchangeKernelId;
    public TransactionSessionId GetTransactionSessionId() => _DataExchangeKernelId.GetTransactionSessionId();

    #endregion
}