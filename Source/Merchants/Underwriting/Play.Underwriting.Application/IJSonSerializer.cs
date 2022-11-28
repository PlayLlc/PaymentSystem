namespace Play.Underwriting.Application;

public interface IJSonSerializer
{
    #region Instance Members

    _T Deserialize<_T>(string input);

    #endregion

    #region Serialization

    string Serialize<_T>(_T obj);

    #endregion
}