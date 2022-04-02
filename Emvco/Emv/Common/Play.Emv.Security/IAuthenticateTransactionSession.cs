using Play.Emv.Ber;

namespace Play.Emv.Security;

public interface IAuthenticateTransactionSession
{
    public void AuthenticateSda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, IReadAndWriteSecuritySessionData sessionSecurityRepository);

    public void AuthenticateDda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, IReadAndWriteSecuritySessionData sessionSecurityRepository);

    public void AuthenticateFirstCda(
        ITlvReaderAndWriter database, ICertificateDatabase certificateDatabase, IReadAndWriteSecuritySessionData sessionSecurityRepository);
}