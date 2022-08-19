using System.Threading.Tasks;

using Play.Emv.Display.Configuration;
using Play.Emv.Display.Contracts;

namespace Play.Emv.Display;

public interface IDisplayMessages
{
    #region Instance Members

    public Task Display(string displayMessage);

    #endregion
}