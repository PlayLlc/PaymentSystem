using Play.Emv.DataElements;

namespace Play.Emv.Security.Messages.Static;

public class AuthenticateStaticDataResponse
{
    #region Instance Values

    private readonly TerminalVerificationResult _TerminalVerificationResult;
    private readonly ErrorIndication _ErrorIndication;

    #endregion

    #region Constructor

    public AuthenticateStaticDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication)

    {
        _TerminalVerificationResult = terminalVerificationResult;
        _ErrorIndication = errorIndication;
    }

    #endregion

    #region Instance Members

    public TerminalVerificationResult GetTerminalVerificationResult()
    {
        return _TerminalVerificationResult;
    }

    public ErrorIndication GetErrorIndication()
    {
        return _ErrorIndication;
    }

    #endregion
}