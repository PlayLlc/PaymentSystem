using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Kernel.Contracts;

public record OutKernelResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(OutKernelResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    private readonly Outcome _Outcome;
    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public OutKernelResponse(CorrelationId correlationId, KernelSessionId sessionId, Outcome outcome) : base(correlationId, MessageTypeId,
        ChannelTypeId)
    {
        _Outcome = outcome;
        _KernelSessionId = sessionId;
    }

    #endregion

    #region Instance Members

    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public Outcome GetOutcome() => _Outcome;

    #endregion
}