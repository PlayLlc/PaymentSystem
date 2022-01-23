using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Messaging;

public class SecurityResponse
{
    #region Instance Values

    private readonly TerminalVerificationResult _TerminalVerificationResult;
    private readonly ErrorIndication _ErrorIndication;

    #endregion

    #region Constructor

    public SecurityResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication)
    {
        _ErrorIndication = errorIndication;
        _TerminalVerificationResult = terminalVerificationResult;
    }

    #endregion

    #region Instance Members

    public TerminalVerificationResult GetTerminalVerificationResult()
    {
        return _TerminalVerificationResult;
    }

    #endregion
}