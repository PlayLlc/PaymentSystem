using System.Threading;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Services;

namespace Play.Emv.Kernel2.Services;

public class Kernel2Process : KernelProcess
{
    #region Constructor

    public Kernel2Process(Kernel2StateMachine kernelStateMachine) : base(kernelStateMachine, new CancellationTokenSource())
    { }

    #endregion

    #region Instance Members

    public override KernelId GetKernelId() => ShortKernelIdTypes.Kernel2;

    public override void Enqueue(StopKernelRequest message)
    {
        // BUG: Possible race condition between these 3 statements
        Clear();
        base.Enqueue(message);
        base.Enqueue(new CleanKernelRequest(message.GetKernelSessionId()));
    }

    #endregion
}