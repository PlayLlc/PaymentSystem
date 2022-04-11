using System;
using System.Collections.Immutable;
using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Security;
using Play.Encryption.Certificates;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Emv.Kernel.Databases;

public partial class KernelDatabase : ICertificateDatabase
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<RegisteredApplicationProviderIndicator, CertificateAuthorityDataset> _Certificates;

    #endregion

    #region Instance Members

    /// <summary>
    ///     Indicates if the <see cref="CaPublicKeyCertificate" /> is currently valid. If the current date and time
    ///     is before the certificate's active date or after the certificate's expiry date, then the certificate is
    ///     revoked. Certificates can also be revoked by the issuer
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex caPublicKeyIndex)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(IsRevoked)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        if (!TryGet(rid, caPublicKeyIndex, out CaPublicKeyCertificate? result))
            return true;

        return result!.IsRevoked();
    }

    public bool IsRevoked(RegisteredApplicationProviderIndicator rid, CertificateSerialNumber serialNumber)
    {
        CertificateAuthorityDataset dataSet = _Certificates.FirstOrDefault(a => a.Key == rid).Value;

        if (!dataSet.TryGet(serialNumber, out CaPublicKeyCertificate? caPublicKeyCertificate))
            return true;

        return caPublicKeyCertificate!.IsRevoked();
    }

    /// <summary>
    ///     Updates the <see cref="ICertificateDatabase" /> by removing any <see cref="CaPublicKeyCertificate" />
    ///     that has expired since the last time they were checked
    /// </summary>
    public void PurgeRevokedCertificates()
    {
        if (!IsActive())
            return;

        for (int i = 0; i < _Certificates.Count; i++)
            _Certificates.ElementAt(i).Value.PurgeRevokedCertificates();
    }

    /// <summary>
    ///     Attempts to get the <see cref="CaPublicKeyCertificate" /> associated with the
    ///     <param name="rid" />
    ///     and
    ///     <param name="index"></param>
    ///     provided. If the <see cref="CaPublicKeyCertificate" /> is revoked or none can be found then the return value will
    ///     be false
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public bool TryGet(RegisteredApplicationProviderIndicator rid, CaPublicKeyIndex index, out CaPublicKeyCertificate? result)
    {
        if (!IsActive())
        {
            throw new TerminalDataException(
                $"The method {nameof(TryGet)} cannot be accessed because the {nameof(KernelDatabase)} is not active");
        }

        if (!_Certificates.TryGetValue(rid, out CertificateAuthorityDataset? dataset))
        {
            throw new TerminalDataException(
                $"The {nameof(KernelDatabase)} does not have a {nameof(CertificateAuthorityDataset)} for the {nameof(RegisteredApplicationProviderIndicator)} value: [{rid}]");
        }

        return dataset.TryGet(index, out result);
    }

    #endregion
}