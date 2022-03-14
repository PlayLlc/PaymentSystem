using Play.Emv.DataElements;
using Play.Emv.Exceptions;
using Play.Emv.Icc;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;
using Play.Emv.Messaging;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class WaitingForEmvModeFirstWriteFlag : KernelState
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
            _KernelDatabase.Update(StatusOutcome.EndApplication);
            _KernelDatabase.Update(Level3Error.Stop);
            _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(),
                                                       _KernelDatabase.GetOutcome()));

            // BUG: I think the book says to clear the database and session on stop but i think our implementation might still use DEK to grab the required data before sending it to the acquirer. Check the pattern in the book and your implementation
            Clear();

            return _KernelStateResolver.GetKernelState(Idle.StateId);
        }

        #endregion
    }
}