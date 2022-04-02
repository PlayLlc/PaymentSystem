using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Security.Exceptions;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Security.Certificates.Factories;

internal partial class CertificateFactory
{
    /// <remarks>EMV Book 2 Section 5.2</remarks>
    /// <exception cref="CryptographicAuthenticationMethodFailedException"></exception>
    public static CaPublicKeyCertificate RecoverCertificateAuthorityCertificate(
        ITlvReaderAndWriter tlvDatabase, ICertificateDatabase certificateDatabase)
    {
        try
        {
            CaPublicKeyIndex index = tlvDatabase.Get<CaPublicKeyIndex>(CaPublicKeyIndex.Tag);
            RegisteredApplicationProviderIndicator rid = tlvDatabase.Get<ApplicationDedicatedFileName>(ApplicationDedicatedFileName.Tag)
                .GetRegisteredApplicationProviderIndicator();

            if (!certificateDatabase.TryGet(rid, index, out CaPublicKeyCertificate? caPublicKey))
            {
                // TODO: Update the transaction flow correctly and set the transaction values rather than throw this error
                throw new
                    CryptographicAuthenticationMethodFailedException($"The {nameof(CaPublicKeyCertificate)} with the {nameof(CaPublicKeyIndex)} value: [{index}] was unavailable for the {nameof(RegisteredApplicationProviderIndicator)}: [{rid}]. Authentication has failed");
            }

            return caPublicKey!;
        }
        catch (TerminalDataException exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
        catch (CryptographicAuthenticationMethodFailedException)
        {
            // TODO: Logging
            throw;
        }
        catch (Exception exception)
        {
            // TODO: Logging
            throw new CryptographicAuthenticationMethodFailedException(exception);
        }
    }
}