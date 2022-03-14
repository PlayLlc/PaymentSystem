using System.Threading.Tasks;

using Play.Core.Math;

namespace Play.Emv.Kernel.Services;

public interface IPercentageSelectionQueue
{
    #region Instance Members

    public Task<bool> IsRandomSelection(Percentage percentage);

    #endregion
}