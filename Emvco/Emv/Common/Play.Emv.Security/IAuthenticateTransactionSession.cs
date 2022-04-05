using Play.Emv.Ber;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Security;

public interface IAuthenticateTransactionSession
{
    public void AuthenticateSda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    public void AuthenticateDda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, StaticDataToBeAuthenticated staticDataToBeAuthenticated);

    public void AuthenticateFirstCda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, GenerateApplicationCryptogramResponse rapdu,
        StaticDataToBeAuthenticated staticDataToBeAuthenticated);
}