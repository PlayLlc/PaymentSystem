using Play.Emv.Identifiers;
using Play.Emv.Messaging;
using Play.Emv.Outcomes;
using Play.Messaging;

namespace Play.Emv.Reader.Contracts.SignalOut;

public record OutReaderResponse : ResponseSignal
{
    #region Static Metadata

    public static readonly MessageTypeId MessageTypeId = CreateMessageTypeId(typeof(OutReaderResponse));
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

    public TransactionSessionId GetTransactionSessionId() => _Outcome.GetTransactionSessionId();
    public bool TryGetKernelSessionId(out KernelSessionId? result) => _Outcome.TryGetKernelSessionId(out result);
    public FinalOutcome GetOutcome() => _Outcome;

    #endregion
}