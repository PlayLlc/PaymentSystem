using System.Text.Json.Serialization;

namespace MockPos.Configuration;

public class CertificateAuthorityConfigurationDto
{
    #region Instance Values

    [JsonPropertyName(nameof(Certificates))] public List<CertificateDto> Certificates { get; set; } = new();

    #endregion
}