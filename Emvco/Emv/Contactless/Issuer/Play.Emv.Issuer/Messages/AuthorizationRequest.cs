using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataObjects;
using Play.Interchange.Messages;
using Play.Interchange.Messages.Body;
using Play.Interchange.Messages.Header;

using Version = Play.Interchange.Messages.Header.Version;

namespace Play.Emv.Issuer.Messages;

internal class AuthorizationRequest : AcquirerMessage
{
    #region Constructor

    private AuthorizationRequest(AcquirerHeader header, AcquirerBody body) : base(header, body)
    { }

    #endregion

    #region Instance Members

    public static void Create()
    {
        MessageTypeIndicator mti = new(Version._1993, Class.Authorization, Function.Request, Origin.Acquirer);
        var bitMap = new Bitmap();
    }

    #endregion

    public class Builder
    {
        #region Instance Values

        private Bitmap _Bitmap;

        #endregion

        #region Constructor

        public Builder(IQueryTlvDatabase tlvDatabase)
        { }

        #endregion
    }
}