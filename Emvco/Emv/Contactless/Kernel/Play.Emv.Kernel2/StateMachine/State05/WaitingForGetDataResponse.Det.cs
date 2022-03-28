using System;

using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGetDataResponse : KernelState
{
    #region DET - QueryTerminalResponse

    // BUG: Need to make sure you're properly implementing each DEK handler for each state

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, QueryTerminalResponse signal)
    {
        HandleRequestOutOfSync(session, signal);
        UpdateDataExchangeSignal(signal);

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #region S5.2

    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>Book C-2 Section S5.2</remarks>
    /// <exception cref="TerminalDataException"></exception>
    private void UpdateDataExchangeSignal(QueryTerminalResponse signal)
    {
        _KernelDatabase.Update(signal.GetDataToSend().AsPrimitiveValues());
    }

    #endregion

    #endregion
}