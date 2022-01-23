using ___TEMP.Play.Emv.Security.Messaging;

using Play.Emv.DataElements;

namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.DynamicDataAuthentication;

public class AuthenticateDynamicDataResponse : SecurityResponse
{
    #region Constructor

    public AuthenticateDynamicDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) :
        base(terminalVerificationResult, errorIndication)
    { }

    #endregion
}