﻿using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine._Temp_LogicalGroup;

public partial class Idle : KernelState
{
    #region STOP

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        _KernelDatabase.Update(Level3Error.Stop);

        _KernelDatabase.Update(StatusOutcome.EndApplication);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), signal.GetKernelSessionId(), _KernelDatabase.GetOutcome()));

        Clear();

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion
}