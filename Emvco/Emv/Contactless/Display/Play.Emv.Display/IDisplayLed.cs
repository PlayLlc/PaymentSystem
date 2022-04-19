using System.Threading.Tasks;

using Play.Emv.Ber;

namespace Play.Emv.Display;

public interface IDisplayLed
{
    #region Instance Members

    public Task Display(Statuses statuses);

    #endregion
}