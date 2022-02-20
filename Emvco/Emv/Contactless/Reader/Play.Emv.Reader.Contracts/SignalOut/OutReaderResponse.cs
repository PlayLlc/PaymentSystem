using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Emv.Sessions;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalOut;

public record OutReaderResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = GetMessageTypeId(typeof(OutReaderResponse));
    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Reader;

    #endregion

    #region Instance Values

    private readonly FinalOutcome _Outcome;

    #endregion

    #region Constructor

    public OutReaderResponse(CorrelationId correlationId, FinalOutcome outcome) : base(correlationId, MessageTypeId, ChannelTypeId)
    {
        _Outcome = outcome;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => GetKernelSessionId().GetTransactionSessionId();
    public KernelSessionId GetKernelSessionId() => _Outcome.GetKernelSessionId();
    public FinalOutcome GetOutcome() => _Outcome;

    #endregion
}