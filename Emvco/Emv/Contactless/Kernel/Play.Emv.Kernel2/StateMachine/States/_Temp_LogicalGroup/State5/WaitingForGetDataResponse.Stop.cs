using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup.State5;

public partial class WaitingForGetDataResponse : KernelState
{
    #region STOP

    public override KernelState Handle(KernelSession session, StopKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    #endregion
}