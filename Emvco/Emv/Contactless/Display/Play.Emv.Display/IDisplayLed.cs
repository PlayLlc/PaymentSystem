using System.Threading.Tasks;

using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;

namespace Play.Emv.Display;

public interface IDisplayLed
{
    #region Instance Members

    public Task Display(Status status);

    #endregion
}