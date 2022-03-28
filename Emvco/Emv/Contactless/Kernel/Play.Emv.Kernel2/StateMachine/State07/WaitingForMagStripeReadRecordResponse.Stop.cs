using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.State;

namespace Play.Emv.Kernel2.StateMachine
{
    public partial class WaitingForMagStripeReadRecordResponse
    {
        #region STOP

        #region S7.7 - S7.8

        public override KernelState Handle(KernelSession session, StopKernelRequest signal)
        {
            HandleRequestOutOfSync(session, signal);

            _KernelDatabase.Update(Level3Error.Stop);
            _KernelDatabase.Update(StatusOutcome.EndApplication);

            // HACK: This is being called twice when a STOP signal is requested by the Kernel State itself
            _KernelDatabase.CreateEmvDiscretionaryData(_DataExchangeKernelService);
            _KernelEndpoint.Send(new OutKernelResponse(session.GetCorrelationId(), session.GetKernelSessionId(),
                                                       _KernelDatabase.GetOutcome()));

            // BUG: I think the book says to clear the database and session on stop but i think our implementation might still use DEK to grab the required data before sending it to the acquirer. Check the pattern in the book and your implementation
            Clear();

            return _KernelStateResolver.GetKernelState(Idle.StateId);
        }

        #endregion

        #endregion
    }
}