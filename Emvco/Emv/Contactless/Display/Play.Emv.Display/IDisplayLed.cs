using System.Threading.Tasks;

using Play.Emv.Ber;
using Play.Emv.Ber.Enums;

namespace Play.Emv.Display;

public interface IDisplayLed
{
    #region Instance Members

    public Task Display(Statuses statuses);

    #endregion
}