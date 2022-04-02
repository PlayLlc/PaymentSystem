using DeleteMe.Exceptions;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Icc.FileSystem.DedicatedFiles;

namespace DeleteMe.Certificates
{
    public partial class CertificateFactory
    {
        internal static class CertificateAuthority
        {
            /// <remarks>EMV Book 2 Section 5.2</remarks>
            /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
            public static CaPublicKeyCertificate GetCaPublicKey(ICertificateDatabase certificateDatabase, IReadTlvDatabase database)
            {
                CaPublicKeyIndex index = database.Get<CaPublicKeyIndex>(CaPublicKeyIndex.Tag);
                RegisteredApplicationProviderIndicator rid = database.Get<ApplicationDedicatedFileName>(ApplicationDedicatedFileName.Tag)
                    .GetRegisteredApplicationProviderIndicator();

                if (!certificateDatabase.TryGet(rid, index, out CaPublicKeyCertificate? caPublicKey))
                {
                    // TODO: Update the transaction flow correctly and set the transaction values rather than throw this error
                    throw new
                        CryptographicAuthenticationMethodFailedException($"The {nameof(CaPublicKeyCertificate)} with the {nameof(CaPublicKeyIndex)} value: [{index}] was unavailable for the {nameof(RegisteredApplicationProviderIndicator)}: [{rid}]. Authentication has failed");
                }

                return caPublicKey;
            }
        }
    }
}