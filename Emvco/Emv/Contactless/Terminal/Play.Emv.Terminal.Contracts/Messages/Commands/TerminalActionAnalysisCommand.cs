using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Sessions;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalActionAnalysisCommand
{
    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;
    private readonly DataObjectListResult _CardRiskManagementDolResult;
    private readonly TerminalVerificationResults _TerminalVerificationResults;
    private readonly DataObjectListResult? _DataStorageDolResult;

    #endregion

    #region Constructor

    public TerminalActionAnalysisCommand(
        TransactionSessionId transactionSessionId,
        TerminalVerificationResults terminalVerificationResults,
        DataObjectListResult cardRiskManagementDolResult,
        DataObjectListResult? dataStorageDolResult)
    {
        _TerminalVerificationResults = terminalVerificationResults;
        _TransactionSessionId = transactionSessionId;
        _CardRiskManagementDolResult = cardRiskManagementDolResult;
        _DataStorageDolResult = dataStorageDolResult;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public DataObjectListResult GetCardRiskManagementDolResult() => _CardRiskManagementDolResult;
    public TerminalVerificationResults GetTerminalVerificationResults() => _TerminalVerificationResults;
    public DataObjectListResult? GetDataStorageDolResult() => _DataStorageDolResult;

    #endregion
}