using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Sessions;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalActionAnalysisCommand
{
    #region Instance Values

    private readonly TransactionSessionId _TransactionSessionId;
    private readonly ActionCodes _IssuerActionCodeDefault;
    private readonly ActionCodes _IssuerActionCodeDenial;
    private readonly ActionCodes _IssuerActionCodeOnline;
    private readonly CryptogramInformationData _CryptogramInformationData;
    private readonly DataObjectListResult _CardRiskManagementDolResult;
    private readonly TerminalVerificationResults _TerminalVerificationResults;
    private readonly DataObjectListResult? _DataStorageDolResult;

    #endregion

    #region Constructor

    public TerminalActionAnalysisCommand(
        TransactionSessionId transactionSessionId,
        TerminalVerificationResults terminalVerificationResults,
        CryptogramInformationData cryptogramInformationData,
        DataObjectListResult cardRiskManagementDolResult,
        DataObjectListResult? dataStorageDolResult,
        ActionCodes issuerActionCodeDefault,
        ActionCodes issuerActionCodeDenial,
        ActionCodes issuerActionCodeOnline)
    {
        _TerminalVerificationResults = terminalVerificationResults;
        _IssuerActionCodeDefault = issuerActionCodeDefault;
        _IssuerActionCodeDenial = issuerActionCodeDenial;
        _IssuerActionCodeOnline = issuerActionCodeOnline;
        _TransactionSessionId = transactionSessionId;
        _CryptogramInformationData = cryptogramInformationData;
        _CardRiskManagementDolResult = cardRiskManagementDolResult;
        _DataStorageDolResult = dataStorageDolResult;
    }

    #endregion

    #region Instance Members

    public TransactionSessionId GetTransactionSessionId() => _TransactionSessionId;
    public CryptogramInformationData GetCryptogramInformationData() => _CryptogramInformationData;
    public DataObjectListResult GetCardRiskManagementDolResult() => _CardRiskManagementDolResult;
    public TerminalVerificationResults GetTerminalVerificationResults() => _TerminalVerificationResults;
    public DataObjectListResult? GetDataStorageDolResult() => _DataStorageDolResult;
    public ActionCodes GetIssuerActionCodeDefault() => _IssuerActionCodeDefault;
    public ActionCodes GetIssuerActionCodeDenial() => _IssuerActionCodeDenial;
    public ActionCodes GetIssuerActionCodeOnline() => _IssuerActionCodeOnline;

    #endregion
}