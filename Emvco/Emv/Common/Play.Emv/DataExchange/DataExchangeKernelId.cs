using Play.Emv.DataElements;
using Play.Emv.Sessions;

namespace Play.Emv.DataExchange;

public readonly record struct DataExchangeKernelId
{
    #region Instance Values

    private readonly ShortKernelIdTypes _ShortKernelIdTypes;
    private readonly KernelSessionId _KernelSessionId;

    #endregion

    #region Constructor

    public DataExchangeKernelId(ShortKernelIdTypes shortKernelIdTypes, KernelSessionId kernelSessionId)
    {
        _ShortKernelIdTypes = shortKernelIdTypes;
        _KernelSessionId = kernelSessionId;
    }

    #endregion

    #region Instance Members

    public ShortKernelIdTypes GetShortKernelId() => _ShortKernelIdTypes;
    public KernelSessionId GetKernelSessionId() => _KernelSessionId;
    public TransactionSessionId GetTransactionSessionId() => _KernelSessionId.GetTransactionSessionId();

    #endregion
}