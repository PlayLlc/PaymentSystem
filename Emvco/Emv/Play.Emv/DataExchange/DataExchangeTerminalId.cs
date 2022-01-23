using Play.Emv.DataElements;
using Play.Emv.Sessions;

namespace Play.Emv.DataExchange;

public readonly record struct DataExchangeTerminalId
{
    #region Instance Values

    private readonly ShortKernelId _ShortKernelId;
    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public DataExchangeTerminalId(ShortKernelId shortKernelId, TransactionSessionId transactionSessionId)
    {
        _ShortKernelId = shortKernelId;
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public ShortKernelId GetShortKernelId() => _ShortKernelId;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}