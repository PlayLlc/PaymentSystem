using Play.Encryption.Signing;

namespace Play.Emv.Security.Certificates.Factories;

internal partial class CertificateFactory
{
    #region Static Metadata

    protected static readonly SignatureService _SignatureService = new();

    #endregion
}