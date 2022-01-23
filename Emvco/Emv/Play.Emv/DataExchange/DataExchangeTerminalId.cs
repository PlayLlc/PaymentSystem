using Play.Emv.DataElements;
using Play.Emv.Sessions;

namespace Play.Emv.DataExchange;

public readonly record struct DataExchangeTerminalId
{
    #region Instance Values

    private readonly KernelId _ShortKernelId;
    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public DataExchangeTerminalId(KernelId shortKernelId, TransactionSessionId transactionSessionId)
    {
        _ShortKernelId = shortKernelId;
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public ShortKernelId GetShortKernelId()
    {
        return _ShortKernelId;
    }

    public TransactionSessionId GetTransactionSessionId()
    {
        return _TransactionSessionId;
    }

    #endregion
}