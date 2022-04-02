namespace DeleteMe.Authentications.Cda;

public interface IAuthenticateCombinedData
{
    #region Instance Members

    public AuthenticateCombinedDataResponse AuthenticateSda(AuthenticateCombinedData1Command command);
    public AuthenticateCombinedDataResponse AuthenticateDda(AuthenticateCombinedData2Command command);
    public AuthenticateCombinedDataResponse AuthenticateFirstCda(AuthenticateCombinedData2Command command);
    public AuthenticateCombinedDataResponse AuthenticateSecondCda(AuthenticateCombinedData2Command command);

    #endregion
}