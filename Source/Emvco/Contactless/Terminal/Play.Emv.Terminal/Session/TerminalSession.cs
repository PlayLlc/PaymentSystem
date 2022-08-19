using Play.Emv.Acquirer.Contracts;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Outcomes;

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