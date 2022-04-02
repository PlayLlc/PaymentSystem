namespace DeleteMe.Authentications.Dda;

public interface IAuthenticateDynamicData
{
    #region Instance Members

    public AuthenticateDynamicDataResponse Authenticate(AuthenticateDynamicDataCommand command);

    #endregion
}