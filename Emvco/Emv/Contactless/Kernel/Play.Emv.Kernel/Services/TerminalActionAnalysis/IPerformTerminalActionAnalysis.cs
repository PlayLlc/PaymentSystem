using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel.Services;

public interface IPerformTerminalActionAnalysis
{
    #region Instance Members

    public GenerateApplicationCryptogramRequest Process(TransactionSessionId sessionId, KernelDatabase database);

    #endregion
}