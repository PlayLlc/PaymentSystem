using System.Text.Json;

namespace Play.Shared.Serializing;

public class SystemTexJsonSerializer : IJSonSerializer
{
    #region Instance Values

    private readonly JsonSerializerOptions _Options;

    #endregion

    #region Constructor

    public SystemTexJsonSerializer()
    {
        _Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    #endregion

    #region Instance Members

    public _ Deserialize<_>(string text)
    {
        return JsonSerializer.Deserialize<_>(text, _Options) ?? throw new Exception("Something went wrong while deserializing");
    }

    #endregion

    #region Serialization

    public string Serialize<_>(_ obj)
    {
        return JsonSerializer.Serialize(obj, _Options);
    }

    #endregion
}