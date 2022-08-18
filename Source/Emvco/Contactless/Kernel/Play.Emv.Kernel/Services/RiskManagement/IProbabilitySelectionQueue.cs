using System.Threading.Tasks;

using Play.Core;

namespace Play.Emv.Kernel.Services;

public interface IProbabilitySelectionQueue
{
    #region Instance Members

    public bool IsRandomSelection(Probability probability);

    #endregion
}