using Play.Encryption.Signing;

namespace DeleteMe.Certificates
{
    internal partial class CertificateFactory
    {
        #region Instance Values

        protected readonly SignatureService _SignatureService = new();

        #endregion
    }
}