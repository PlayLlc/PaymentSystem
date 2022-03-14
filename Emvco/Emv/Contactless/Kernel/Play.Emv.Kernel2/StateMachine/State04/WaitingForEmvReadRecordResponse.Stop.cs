using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForEmvReadRecordResponse : KernelState
{
    #region STOP

    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Kernel.Exceptions.TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        _KernelDatabase.Update(Level3Error.Stop);

        _KernelDatabase.Update(StatusOutcome.EndApplication);

        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _KernelDatabase.GetOutcome()));

        Clear();

        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }

    #endregion
}