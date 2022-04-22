using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Kernel.Services;

public interface IPerformTerminalActionAnalysis
{
    #region Instance Members

    public CryptogramTypes Process(TransactionSessionId sessionId, KernelDatabase database);

    #endregion
}