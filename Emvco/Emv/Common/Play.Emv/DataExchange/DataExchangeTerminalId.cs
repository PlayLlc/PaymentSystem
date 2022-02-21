using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Sessions;

namespace Play.Emv.DataExchange;

public readonly record struct DataExchangeTerminalId
{
    #region Instance Values

    private readonly KernelId KernelId;
    private readonly TransactionSessionId _TransactionSessionId;

    #endregion

    #region Constructor

    public DataExchangeTerminalId(KernelId kernelId, TransactionSessionId transactionSessionId)
    {
        KernelId = kernelId;
        _TransactionSessionId = transactionSessionId;
    }

    #endregion

    #region Instance Members

    public KernelId GetKernelId() => KernelId;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;

    #endregion
}