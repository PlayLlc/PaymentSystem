using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine;

public partial class WaitingForGpoResponse : KernelState
{
    #region STOP

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public override KernelState Handle(KernelSession session, StopKernelRequest signal)
    {
        HandleRequestOutOfSync(session, signal);

        _KernelDatabase.Update(Level3Error.Stop);

        _KernelDatabase.Update(StatusOutcome.EndApplication);

        _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);

        _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(), _KernelDatabase.GetOutcome()));

        // BUG: I think the book says to clear the database and session on stop but i think our implementation might still use DEK to grab the required data before sending it to the acquirer. Check the pattern in the book and your implementation
        Clear();

        return _KernelStateResolver.GetKernelState(Idle.StateId);
    }

    #endregion
}