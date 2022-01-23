using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Security.Messaging;

namespace Play.Emv.Security.Authentications.DynamicDataAuthentication;

public class AuthenticateDynamicDataResponse : SecurityResponse
{
    #region Constructor

    public AuthenticateDynamicDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) : base(
        terminalVerificationResult, errorIndication)
    { }

    #endregion
}