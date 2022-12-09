using System.Runtime.InteropServices;

namespace Play.Domain;

public interface IDto
{
    #region Instance Members

    // public string AsJson(){}
    public dynamic GetId();
    public Dictionary<string, object> GetProperties();

    #endregion
}