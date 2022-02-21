using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Interchange.Messages;
using Play.Interchange.Messages.Body;
using Play.Interchange.Messages.Header;

namespace Play.Emv.Acquirer.Messages;

public class AuthorizationRequest : AcquirerMessage
{
    #region Constructor

    public AuthorizationRequest(AcquirerHeader header, AcquirerBody body) : base(header, body)
    { }

    #endregion
}