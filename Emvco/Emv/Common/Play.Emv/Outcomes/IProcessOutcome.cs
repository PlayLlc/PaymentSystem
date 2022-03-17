using Play.Emv.Identifiers;
using Play.Messaging;

namespace Play.Emv.Outcomes;

public interface IProcessOutcome
{
    #region Instance Members

    public void Process(CorrelationId correlationId, KernelSessionId sessionId, Transaction transaction);
    public void Process(CorrelationId correlationId, TransactionSessionId sessionId, Transaction transaction);

    #endregion
}