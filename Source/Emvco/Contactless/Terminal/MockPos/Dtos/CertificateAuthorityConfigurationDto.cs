using System.Linq.Expressions;
using System.Text.Json.Serialization;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Security;
using Play.Icc.FileSystem.DedicatedFiles;

namespace MockPos.Dtos;

public class CertificateAuthorityConfigurationDto
{
    #region Instance Values

    [JsonPropertyName(nameof(Certificates))]
    public List<CertificateDto> Certificates { get; set; } = new();

    #endregion

    #region Serialization

    public CertificateAuthorityDataset[] Decode()
    {
        List<CaPublicKeyCertificate> caPublicKeyCertificates = Certificates.Select(a => a.Decode()).ToList();
        List<RegisteredApplicationProviderIndicator> rids = caPublicKeyCertificates.Select(a => a.GetRegisteredApplicationProviderIndicator()).Distinct()
            .ToList();
        List<CertificateAuthorityDataset> certificates = new();

        foreach (RegisteredApplicationProviderIndicator rid in rids)
        {
            CaPublicKeyCertificate[] certificateByRid = caPublicKeyCertificates.Where(b => b.GetRegisteredApplicationProviderIndicator() == rid).ToArray();
            CertificateAuthorityDataset dataset = new(rid, certificateByRid);
            certificates.Add(dataset);
        }

        return certificates.ToArray();
    }

    #endregion
}