using System.Text.Json.Serialization;

namespace MockPos.Dtos;

public class CertificateAuthorityConfigurationDto
{
    #region Instance Values

    [JsonPropertyName(nameof(Certificates))]
    public List<CertificateDto> Certificates { get; set; } = new();

    #endregion
}