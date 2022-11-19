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

    public T Deserialize<T>(string text)
    {
        return JsonSerializer.Deserialize<T>(text, _Options) ?? throw new Exception("Something went wrong while deserializing");
    }

    #endregion

    #region Serialization

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, _Options);
    }

    #endregion
}