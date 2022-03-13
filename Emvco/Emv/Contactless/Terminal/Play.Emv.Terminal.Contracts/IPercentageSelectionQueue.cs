using System.Threading.Tasks;

using Play.Core.Math;

namespace Play.Emv.Terminal.Contracts;

public interface IPercentageSelectionQueue
{
    #region Instance Members

    public Task<bool> IsRandomSelection(Percentage percentage);

    #endregion
}