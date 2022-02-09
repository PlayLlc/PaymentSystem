using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;

namespace Play.Emv.Kernel2.Services;

internal class Kernel2StateLock
{
    #region Instance Values

    public Kernel2Session? Session;
    public KernelState KernelState;

    #endregion

    #region Constructor

    public Kernel2StateLock(KernelState state)
    {
        KernelState = state;
    }

    #endregion
}