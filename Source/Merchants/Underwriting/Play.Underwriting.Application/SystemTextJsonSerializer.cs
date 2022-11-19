using System.Text.Json;

namespace Play.Shared.Serializing;

public class SystemTexJsonSerializer : IJSonSerializer
{

    private readonly JsonSerializerOptions _Options;

    public SystemTexJsonSerializer()
    {
        _Options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public T Deserialize<T>(string text) => JsonSerializer.Deserialize<T>(text, _Options) ?? throw new Exception("Something went wrong while deserializing");

    public string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, _Options);
}
