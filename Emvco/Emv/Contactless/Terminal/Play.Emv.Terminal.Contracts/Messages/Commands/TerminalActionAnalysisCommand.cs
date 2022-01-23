using Play.Emv.DataElements;

namespace Play.Emv.Terminal.Contracts.Messages.Commands;

public class TerminalActionAnalysisCommand
{
    #region Instance Values

    private readonly ActionCodes _IssuerActionCodeDefault;
    private readonly ActionCodes _IssuerActionCodeDenial;
    private readonly ActionCodes _IssuerActionCodeOnline;
    private readonly TerminalVerificationResults _TerminalVerificationResults;

    #endregion

    #region Constructor

    public TerminalActionAnalysisCommand(
        TerminalVerificationResults terminalVerificationResults,
        ActionCodes issuerActionCodeDefault,
        ActionCodes issuerActionCodeDenial,
        ActionCodes issuerActionCodeOnline)
    {
        _TerminalVerificationResults = terminalVerificationResults;
        _IssuerActionCodeDefault = issuerActionCodeDefault;
        _IssuerActionCodeDenial = issuerActionCodeDenial;
        _IssuerActionCodeOnline = issuerActionCodeOnline;
    }

    #endregion

    #region Instance Members

    public ActionCodes GetIssuerActionCodeDefault() => _IssuerActionCodeDefault;
    public ActionCodes GetIssuerActionCodeDenial() => _IssuerActionCodeDenial;
    public ActionCodes GetIssuerActionCodeOnline() => _IssuerActionCodeOnline;
    public TerminalVerificationResults GetTerminalVerificationResults() => _TerminalVerificationResults;

    #endregion
}