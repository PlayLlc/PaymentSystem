using System.Text.Json.Serialization;

namespace MockPos.Configuration;

public class DisplayMessageSetDto
{
    #region Instance Values

    public string? LanguageCode { get; set; }
    public string? CountryCode { get; set; }

    [JsonPropertyName(nameof(DisplayMessages))]
    public List<DisplayMessageDto> DisplayMessages { get; set; } = new();

    #endregion
}