using System;

using Play.Emv.Exceptions;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup.State5;

public partial class WaitingForGetDataResponse : KernelState
{
    #region DET

    public override KernelState Handle(KernelSession session, QueryKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    public override KernelState Handle(KernelSession session, UpdateKernelRequest signal) =>
        throw new RequestOutOfSyncException(signal, ChannelType.Kernel);

    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);
        UpdateDataExchangeSignal(signal);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #region S5.2

    /// <exception cref="InvalidOperationException"></exception>
    private void UpdateDataExchangeSignal(QueryTerminalResponse signal)
    {
        _KernelDatabase.UpdateRange(signal.GetDataToSend().AsTagLengthValueArray());
    }

    #endregion

    #endregion
}