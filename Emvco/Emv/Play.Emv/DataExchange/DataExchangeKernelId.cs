using Play.Emv.DataElements;
using Play.Emv.Sessions;

namespace Play.Emv.DataExchange;

public readonly record struct DataExchangeKernelId
{
    #region Instance Values

    private readonly ShortKernelId _ShortKernelId;
    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public DataExchangeKernelId(ShortKernelId shortKernelId, KernelSessionId kernelSessionId)
    {
        _ShortKernelId = shortKernelId;
        _KernelSessionId = kernelSessionId;
    }

    #endregion

    #region Instance Members

    public ShortKernelId GetShortKernelId() => _ShortKernelId;
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    #endregion
}