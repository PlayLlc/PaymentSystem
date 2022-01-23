using Play.Emv.DataElements;
using Play.Emv.DataExchange;
using Play.Emv.Messaging;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts.SignalIn;

/// <summary>
///     This message clears the log of <see cref="TornRecord" /> from previous transactions
/// </summary>
public record CleanKernelRequest : RequestSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(CleanKernelRequest));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public CleanKernelRequest(KernelSessionId kernelSessionId) : base(MessageTypeId, ChannelTypeId)
    {
        _KernelSessionId = kernelSessionId;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();
    public ShortKernelId GetShortKernelId() => _KernelSessionId.GetKernelId();

    #endregion
}