using DeleteMe.Certificates;

using Play.Emv.Ber;

namespace DeleteMe.Authentications.Dda;

public interface IAuthenticateDynamicData
{
    #region Instance Members

    public void Authenticate(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    #endregion
}