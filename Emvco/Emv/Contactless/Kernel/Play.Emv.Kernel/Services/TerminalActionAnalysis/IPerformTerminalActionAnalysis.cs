using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Kernel.Services;

public interface IPerformTerminalActionAnalysis
{
    #region Instance Members

    public void Process(TransactionSessionId sessionId, KernelDatabase database)

    #endregion
}