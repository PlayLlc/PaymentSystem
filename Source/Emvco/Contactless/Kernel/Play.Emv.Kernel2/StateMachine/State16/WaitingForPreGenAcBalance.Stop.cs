using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForPreGenAcBalance
{
    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        _Database.Update(Level3Error.Stop);

        _Database.Update(StatusOutcomes.EndApplication);
        _Database.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _EndpointClient.Send(new OutKernelResponse(session.GetCorrelationId(), signal.GetKernelSessionId(), _Database.GetTransaction()));

        // BUG: I think the book says to clear the database and session on stop but i think our implementation might still use DEK to grab the required data before sending it to the acquirer. Check the pattern in the book and your implementation
        Clear();

        // CHECK: See how you're handling your OUT signal. We probably need to return to the IDLE state here
        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }
}