using System.Text.Json.Serialization;

namespace MockPos.Configuration;

public class DisplayConfigurationDto
{
    #region Instance Values

    public string? MessageHoldTime { get; set; }

    [JsonPropertyName(nameof(DisplayMessageSets))]
    public List<DisplayMessageSetDto> DisplayMessageSets { get; set; } = new();

    #endregion
}