using System;

using Play.Emv.DataElements.Emv;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPdolData : KernelState
{
    #region STOP

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="session"></param>
    /// <param name="signal"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        OutcomeParameterSet.Builder builder = OutcomeParameterSet.GetBuilder();
        builder.Set(StatusOutcome.EndApplication);
        _KernelDatabase.Update(builder);

        if (!_KernelDatabase.GetErrorIndication().IsErrorPresent())
            _KernelDatabase.Update(Level3Error.Stop);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), signal.GetKernelSessionId(), _KernelDatabase.GetOutcome()));

        Clear();

        return _KernelStateResolver.GetKernelState(StateId);
    }

    #endregion
}