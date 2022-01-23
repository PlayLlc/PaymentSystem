using ___TEMP.Play.Emv.Security.Authentications.Dynamic.CombinedDataAuthentication;
using ___TEMP.Play.Emv.Security.Authentications.Dynamic.DynamicDataAuthentication;

namespace ___TEMP.Play.Emv.Security.Authentications;

public interface IAuthenticateOfflineData
{
    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command);
    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData1Command command);
    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData2Command command);

    #endregion
}