using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

/// <summary>
///     The message contains TLV encoded data objects sent from the terminal. The Reader will add the TLV data objects
///     encoded in <see cref="DataToSend" /> and add them to the TLV Database
/// </summary>
public record UpdateKernelRequest : RequestSignal, IExchangeDataWithTheTerminal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(UpdateKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly DataToSend _DataToSend;
    private readonly DataExchangeTerminalId _DataExchangeTerminalId;

    #endregion

    #region Constructor

    public UpdateKernelRequest(DataExchangeTerminalId dataExchangeTerminalId, DataToSend dataToSend) : base(MessageTypeId, ChannelTypeId)
    {
        _DataToSend = dataToSend;
        _DataExchangeTerminalId = dataExchangeTerminalId;
    }

    #endregion

    #region Instance Members

    public TagLengthValue[] GeTagLengthValueArray() => _DataToSend.AsTagLengthValueArray();
    public DataToSend GetDataToSend() => _DataToSend;
    public DataExchangeTerminalId GetDataExchangeTerminalId() => _DataExchangeTerminalId;
    public TransactionSessionId GetTransactionSessionId() => _DataExchangeTerminalId.GetTransactionSessionId();
    public KernelId GetShortKernelId() => _DataExchangeTerminalId.GetKernelId();

    #endregion
}