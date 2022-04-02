using DeleteMe.Certificates;

using Play.Emv.Ber;
using Play.Emv.Pcd.Contracts;

namespace DeleteMe.Authentications.Cda;

public interface IAuthenticateCombinedData
{
    #region Instance Members

    public void AuthenticateFirstCda(
        GenerateApplicationCryptogramResponse rapdu, ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    #endregion
}