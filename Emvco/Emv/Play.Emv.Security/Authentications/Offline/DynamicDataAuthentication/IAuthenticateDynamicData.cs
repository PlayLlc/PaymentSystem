using Play.Emv.Security.Messages.DDA;

namespace Play.Emv.Security.Authentications.Offline.DynamicDataAuthentication;

public interface IAuthenticateDynamicData
{
    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command);

    #endregion
}