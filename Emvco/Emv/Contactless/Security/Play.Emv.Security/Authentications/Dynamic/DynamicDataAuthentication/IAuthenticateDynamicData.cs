namespace Play.Emv.Security.Authentications.DynamicDataAuthentication;

public interface IAuthenticateDynamicData
{
    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command);

    #endregion
}