using Play.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Security.Messaging;

namespace Play.Emv.Security.Authentications.Static;

public class AuthenticateStaticDataResponse : SecurityResponse
{
    #region Constructor

    public AuthenticateStaticDataResponse(TerminalVerificationResult terminalVerificationResult, ErrorIndication errorIndication) : base(
        terminalVerificationResult, errorIndication)
    { }

    #endregion
}