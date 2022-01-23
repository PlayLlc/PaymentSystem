using Play.Emv.Transactions;
using Play.Messaging;

namespace Play.Emv.Outcomes;

public interface IProcessOutcome
{
    #region Instance Members

    public void Process(CorrelationId correlationId, Transaction transaction);

    #endregion
}