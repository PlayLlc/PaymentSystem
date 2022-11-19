namespace Play.Shared.Serializing;

public interface IJSonSerializer
{
    string Serialize<_T>(_T obj);

    _T Deserialize<_T>(string input);
}
