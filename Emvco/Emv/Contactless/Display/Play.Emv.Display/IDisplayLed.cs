using System.Threading.Tasks;

using Play.Emv.DataElements.Emv.ValueTypes;

namespace Play.Emv.Display;

public interface IDisplayLed
{
    #region Instance Members

    public Task Display(Status status);

    #endregion
}