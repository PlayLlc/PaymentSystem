using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Security.Messaging;

namespace Play.Emv.Security.Authentications;

public class AuthenticationResponse : SecurityResponse
{
    #region Constructor

    public AuthenticationResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) : base(
        terminalVerificationResult, errorIndication)
    { }

    #endregion
}