using ___TEMP.Play.Emv.Security.Messaging;

using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Authentications.Static;

public class AuthenticateStaticDataResponse : SecurityResponse
{
    #region Constructor

    public AuthenticateStaticDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) :
        base(terminalVerificationResult, errorIndication)
    { }

    #endregion
}