using System.Threading.Tasks;

using Play.Core;

namespace Play.Emv.Kernel.Services;

public interface IProbabilitySelectionQueue
{
    #region Instance Members

    public Task<bool> IsRandomSelection(Probability probability);

    #endregion
}