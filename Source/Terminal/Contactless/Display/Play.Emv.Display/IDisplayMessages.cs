using System.Threading.Tasks;

namespace Play.Emv.Display;

public interface IDisplayMessages
{
    #region Instance Members

    public Task Display(string displayMessage);

    #endregion
}