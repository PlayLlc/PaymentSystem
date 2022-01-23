namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.DynamicDataAuthentication;

public interface IAuthenticateDynamicData
{
    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command);

    #endregion
}