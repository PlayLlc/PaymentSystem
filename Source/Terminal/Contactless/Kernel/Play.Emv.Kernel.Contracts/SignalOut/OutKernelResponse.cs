using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

public record OutKernelResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(OutKernelResponse));
    public static readonly ChannelTypeId ChannelTypeId = KernelChannel.Id;

    #endregion

    #region Instance Values

    private readonly Transaction _Transaction;
    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public OutKernelResponse(CorrelationId correlationId, KernelSessionId sessionId, Transaction transaction) : base(correlationId, MessageTypeId,
        ChannelTypeId)
    {
        _Transaction = transaction;
        _KernelSessionId = sessionId;
    }

    #endregion

    #region Instance Members

    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public Outcome GetOutcome() => _Transaction.GetOutcome();
    public Transaction GetTransaction() => _Transaction;

    #endregion
}