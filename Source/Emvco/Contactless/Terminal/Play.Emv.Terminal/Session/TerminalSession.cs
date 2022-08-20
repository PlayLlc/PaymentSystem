using Play.Emv.Identifiers;

namespace Play.Emv.Terminal.Session;

public class TerminalSession
{
    #region Instance Values

    public readonly TransactionSessionId TransactionSessionId;

    #endregion

    #region Constructor

    public TerminalSession(TransactionSessionId transactionSessionId)
    {
        TransactionSessionId = transactionSessionId;
    }

    #endregion
}