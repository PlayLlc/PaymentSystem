using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;

namespace Play.Emv.DataExchange;

public readonly record struct DataExchangeTerminalId
{
    #region Instance Values

    private readonly KernelId _KernelId;
    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public DataExchangeTerminalId(KernelId kernelId, TransactionSessionId transactionSessionId)
    {
        _KernelId = kernelId;
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public KernelId GetKernelId() => _KernelId;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}