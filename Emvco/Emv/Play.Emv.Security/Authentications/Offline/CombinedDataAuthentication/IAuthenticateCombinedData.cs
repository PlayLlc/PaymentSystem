namespace ___TEMP.Play.Emv.Security.Authentications.Dynamic.CombinedDataAuthentication;

public interface IAuthenticateCombinedData
{
    #region Instance Members

    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData1Command command);
    public AuthenticateCombinedDataResponse Authenticate(AuthenticateCombinedData2Command command);

    #endregion
}