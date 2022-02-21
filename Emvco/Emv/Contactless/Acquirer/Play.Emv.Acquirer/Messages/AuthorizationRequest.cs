using Play.Interchange.Messages;
using Play.Interchange.Messages.Body;

namespace Play.Emv.Acquirer.Messages;

public class AuthorizationRequest : InterchangeMessage
{
    #region Constructor

    public AuthorizationRequest(AcquirerHeader header, AcquirerBody body) : base(header, body)
    { }

    #endregion
}