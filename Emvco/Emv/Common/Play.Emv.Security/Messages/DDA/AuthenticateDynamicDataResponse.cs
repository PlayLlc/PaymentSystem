using Play.Emv.DataElements;

namespace Play.Emv.Security.Messages.DDA;

public class AuthenticateDynamicDataResponse
{
    #region Instance Values

    private readonly TerminalVerificationResult _TerminalVerificationResult;
    private readonly ErrorIndication _ErrorIndication;

    #endregion

    #region Constructor

    public AuthenticateDynamicDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication)
    {
        _TerminalVerificationResult = terminalVerificationResult;
        _ErrorIndication = errorIndication;
    }

    #endregion

    #region Instance Members

    public TerminalVerificationResult GetTerminalVerificationResult() => _TerminalVerificationResult;
    public ErrorIndication GetErrorIndication() => _ErrorIndication;

    #endregion
}