using Play.Emv.Ber.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace DeleteMe._Temp
{
    public abstract partial class KernelDatabase
    {
        #region Instance Members

        /// <summary>
        ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
        ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
        ///     revoked. Certificates can also be revoked by the issuer
        /// </summary>
        /// <exception cref="TerminalDataException"></exception>
        public virtual bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(IsRevoked)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
            }

            return _CertificateDatabase!.IsRevoked(rid, caPublicKeyIndex);
        }

        /// <summary>
        ///     Updates the <see cref="ICertificateDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
        ///     that has expired since the last time they were checked
        /// </summary>
        public virtual void PurgeRevokedCertificates()
        {
            if (!IsActive())
                return;

            _CertificateDatabase.PurgeRevokedCertificates();
        }

        /// <summary>
        ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
        ///     <param name="rid" />
        ///     and
        ///     <param name="index"></param>
        ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none
        ///     can be found then the return value will be false
        /// </summary>
        /// <exception cref="TerminalDataException"></exception>
        public virtual bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
        {
            if (!IsActive())
            {
                throw new
                    TerminalDataException($"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
            }

            return _CertificateDatabase.TryGet(rid, index, out result);
        }

        #endregion
    }
}