using Play.Emv.Security.Messages.CDA;
using Play.Emv.Security.Messages.DDA;

namespace Play.Emv.Security;

public interface IAuthenticateOfflineData
{
    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command);
    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData1Command command);
    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData2Command command);

    #endregion
}