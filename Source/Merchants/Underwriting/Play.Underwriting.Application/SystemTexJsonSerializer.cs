using System.Text.Json;

namespace Play.Underwriting.Application;

public class SystemTexJsonSerializer : IJSonSerializer
{
    #region Instance Values

    private readonly JsonSerializerOptions _Options;

    #endregion

    #region Constructor

    public SystemTexJsonSerializer()
    {
        _Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    #endregion

    #region Instance Members

    public _ Deserialize<_>(string text) => JsonSerializer.Deserialize<_>(text, _Options) ?? throw new("Something went wrong while deserializing");

    #endregion

    #region Serialization

    public string Serialize<_>(_ obj) => JsonSerializer.Serialize(obj, _Options);

    #endregion
}