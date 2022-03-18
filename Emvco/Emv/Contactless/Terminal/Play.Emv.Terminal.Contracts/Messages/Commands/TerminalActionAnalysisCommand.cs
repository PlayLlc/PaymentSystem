using Play.Emv.Ber;
using Play.Emv.DataElements;
using Play.Emv.Identifiers;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalActionAnalysisCommand
{
    #region Instance Values

    private readonly ApplicationInterchangeProfile _ApplicationInterchangeProfile;
    private readonly TransactionSessionId _TransactionSessionId;
    private readonly OutcomeParameterSet _OutcomeParameterSet;
    private readonly DataObjectListResult _CardRiskManagementDolResult;
    private readonly TerminalVerificationResults _TerminalVerificationResults;
    private readonly DataObjectListResult? _DataStorageDolResult;

    #endregion

    #region Constructor

    public TerminalActionAnalysisCommand(
        TransactionSessionId transactionSessionId,
        OutcomeParameterSet outcomeParameterSet,
        TerminalVerificationResults terminalVerificationResults,
        ApplicationInterchangeProfile applicationInterchangeProfile,
        DataObjectListResult cardRiskManagementDolResult,
        DataObjectListResult? dataStorageDolResult)
    {
        _TerminalVerificationResults = terminalVerificationResults;
        _TransactionSessionId = transactionSessionId;
        _CardRiskManagementDolResult = cardRiskManagementDolResult;
        _DataStorageDolResult = dataStorageDolResult;
        _ApplicationInterchangeProfile = applicationInterchangeProfile;
        _OutcomeParameterSet = outcomeParameterSet;
    }

    #endregion

    #region Instance Members

    public ApplicationInterchangeProfile GetApplicationInterchangeProfile() => _ApplicationInterchangeProfile;
    public OutcomeParameterSet GetOutcomeParameterSet() => _OutcomeParameterSet;
    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public DataObjectListResult GetCardRiskManagementDolResult() => _CardRiskManagementDolResult;
    public TerminalVerificationResults GetTerminalVerificationResults() => _TerminalVerificationResults;
    public DataObjectListResult? GetDataStorageDolResult() => _DataStorageDolResult;

    #endregion
}