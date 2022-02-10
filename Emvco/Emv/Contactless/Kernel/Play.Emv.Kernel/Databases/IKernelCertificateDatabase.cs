﻿using Play.Emv.DataElements;
using Play.Emv.Security.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public interface IKernelCertificateDatabase
{
    #region Instance Members

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex);

    /// <summary>
    ///     Updates the <see cref="IKernelCertificateDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
    ///     that has expired since the last time they were checked
    /// </summary>
    public void PurgeRevokedCertificates();

    /// <summary>
    ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
    ///     <param name="rid" />
    ///     and
    ///     <param name="index"></param>
    ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none
    ///     can be found then the return value will be false
    /// </summary>
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result);

    #endregion
}