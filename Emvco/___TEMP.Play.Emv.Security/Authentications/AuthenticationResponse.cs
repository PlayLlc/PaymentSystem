using ___TEMP.Play.Emv.Security.Messaging;

using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Authentications;

public class AuthenticationResponse : SecurityResponse
{
    #region Constructor

    public AuthenticationResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) :
        base(terminalVerificationResult, errorIndication)
    { }

    #endregion
}